using FluentNHibernate.Conventions;
using FluentNHibernate.Utils;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;
using payCoreHW3.Context;
using payCoreHW3.Models;

namespace payCoreHW3.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KmeansController : ControllerBase
{
    private readonly IMapperSession _session;
    
    // injection
    public KmeansController(IMapperSession session)
    {
        _session = session;
    }

    [HttpPost("K-MeansClustering")]
    public IActionResult KMeansClustering(long vehicleId, int numberOfClusters)
    {
        // Take all container belongs to vehicleId
        var containers = _session.Containers.Where(x => x.VehicleId == vehicleId).ToArray();

        // Check containers has item or not
        if (containers.Length == 0) return BadRequest("Container not found.");
        // check given numberOfClusters 0 or less.
        if (numberOfClusters <= 0) return BadRequest("numberOfClusters can not be 0 or less.");
        // check given numberOfClusters bigger than container size.
        if (numberOfClusters > containers.Length)
            return BadRequest("numberOfClusters can not be bigger than container size.");

        // Taking coordinates(Latitude for X coordinate,Longitude for Y ), we have two-dimensions (X,Y)
        var containerCoordinates = containers
            .Select(container => new[] { (double)container.Latitude, (double)container.Longitude }).ToArray();
        // Creating random object
        var random = new Random(42);

        //**************************************************************************
        // Yıldızlar icerisine aldigim bolum biraz karışık o yuzden türkçe yazıyorum.
        /*
         Öncelikle random metodu uzerinden bir degerlendirme yapacak olursam ve orneklerle gidecek olursak,
        3 adet kume ve 6 adet containerimiz olsun.
        Verilen 6 adet containerin K-Means algoritmasi geregi rastgele 3 gruba ayirmamiz gerekiyor fakat.
        random degeri 0-1-2 olarak rastgele kumeIndexi degeri olusturacagi icin dusuk item sayisi olan containerlar icin
        bu degerlerden(0-1-2) kumeleme yaparken rastgele sececegi icin bir tanesini hic uretmeyebilir. Yani ornek verecek olursam.
        
        containerlistemdeki 6 eleman -> c1,c2,c3,c4,c5,c6 olsun bunlar icin rastgele kumeleme yapacagimda bekledigim sonuc
        
        [c1,c4][c2,c5][c3,c6] gibi bir sonuctur fakat biz rastgele kume indexi verecegimiz icin bu sonucun;
        
        [c1,c4,c6,c2][][c3,c5] gibi 1(1. index ve 2.grup) degerini hic bir containera vermeme ihtimali var bu yuzden.
        
        
        gelen ilk 3 container(3 sayisi istendigi icin soyluyorum normalde n); 
        c1->0.indexli gruba(1.grup)
        c2->1.indexli gruba(2.grup)
        c3->2.indexli gruba(3.grup)
        
        olarak manuel atama yapilir. Bu sayede istenilen 3 kumeyi olusturmus oluruz.
        Geri kalan c4,c5,c6 containerlari ise rastgele 0-1-2 sayilari olusturularak kumelere dagitilir.
        
        asagidaki resultCluster istenilen n küme sayisina gore bu islemi yapar n tane elemani basta kumelere atar(bos kume olmamasi icin)
        sonrasinda gelen containerClusterRandomly n tane elemandan sonra (bunlar container karistirilmasin 10 kume isteyebiliriz fakat 100
        containerimiz varsa ilk 10'u kumelere yerlestirildikten sonra 11.den baslayarak 100.ye kadar) diger elemanlar icin rastgele kume atamasi yapar.
        
        ve bunlar bir liste haline getirilir. 
        
        Bu listelerin icerisinde ClusterIndex-> Kumeleme icin
        Coordinate ->(x=latitude,y=longitude) Koordinatlar icin
        Container -> Container bilgisini saklayip sonrasinda result olarak dondurebilmek icin.
        
        Ek Not : Container listesini OrderBy ve new Guild yaparak karistirabiliriz.(sonradan arastirip buldugum icin koda eklemedim.)
         */

        var resultCluster = Enumerable
            .Range(0, numberOfClusters)
            .Select(index => (
                    ClusterIndex: index,
                    Coordinate: containerCoordinates[index],
                    Container: containers[index]
                )
            )
            .ToList();
        var containerClusterRandomly = Enumerable
            .Range(0, containerCoordinates.Length - numberOfClusters)
            .Select(index =>
                (ClusterIndex: random.Next(0, numberOfClusters),
                    Coordinate: containerCoordinates[index + numberOfClusters],
                    Container: containers[numberOfClusters + index])).ToList();
        //**************************************************************************
        // Combine two list to one list.
        resultCluster.AddRange(containerClusterRandomly);


        var dimensions = containerCoordinates[0].Length; // Dimensions (x,y) -> 2
        var timeout = 9999999999999999999; // Timeout for our while loop(should not run forever), it has to be limit.
        var isUpdated = true; // condition for change clusters
        var isCentroidInitialized = false;
        var controlArray = Enumerable.Range(0, numberOfClusters).ToArray();
        while (--timeout > 0)
        {
            // Check is there any missing centroid clusterIndex, if it does then the next step will not execute because
            // if there is no point with x(which is 0<x<=n) clusterIndex then program can not calculate the average of centroid coordinates.
            //because we have no point for that centroid.
            if (isCentroidInitialized)
            {
            // Taking indexes in current resultCluster
                var clusterIndexArray = resultCluster.Select(n=>
                    n.ClusterIndex
                     
                ).ToArray();
                //Duplicated numbers are blocked using distinct
                clusterIndexArray = clusterIndexArray.Distinct().ToArray();
                //ordered array
                var orderedClusterIndex= clusterIndexArray.OrderBy(x => x);
             
                var isIn = orderedClusterIndex.SequenceEqual(controlArray);
                // check included or not
                 if (!isIn) return Ok($"{numberOfClusters} is not suitable to cluster containers.");
            }
            
            try
            {
            // Calculate centroids
                 var centroids = Enumerable.Range(0, numberOfClusters)
                    .AsParallel()
                    .Select(index =>
                        (
                            Cluster: index,
                            CentroidCoordinate: Enumerable.Range(0, dimensions)
                                .Select(axis => resultCluster.Where(x => x.ClusterIndex == index)
                                    .Average(x => x.Coordinate[axis]))
                                .ToArray())
                    ).ToArray();
                 isCentroidInitialized = true;

                isUpdated = false; // set false before opereation starts 
                //for loop
                Parallel.For(0, resultCluster.Count, i =>
                {
                    // take item
                    var item = resultCluster[i];
                    // take ClusterIndex of item
                    var currentClusterIndexOfItem = item.ClusterIndex;
                    //check the distance to centroids and take nearest centainers ClusterIndex
                    var newClusterIndexOfItem = centroids.Select(n => (ClusterIndex: n.Cluster,
                            Distance: CalculateDistance(item.Coordinate, n.CentroidCoordinate))).MinBy(x => x.Distance)
                        .ClusterIndex;

                    // Check items clusterIndex changed or not
                    if (newClusterIndexOfItem != currentClusterIndexOfItem)
                    {
                        resultCluster[i] = (newClusterIndexOfItem, item.Coordinate, item.Container);
                        // if items clusterIndex changes then isUpdated becomes true, if its true then we keep going to our algorithm
                        isUpdated = true;
                    }
                    // if items clusterIndex not change then the all items are clustered to nearest centroids. And the algorithm has done.
                });
                // Above, written condition, if there is no updated clusterIndex then program goes outside of while loop.
                if (!isUpdated)
                {
                    break;
                }
            }

            catch (Exception e)
            {

                throw;
            }
        } // while

        // modify resultCluster
        var resultClusterModified = resultCluster.Select(n => new
        {
            n.ClusterIndex, n.Container
        }).ToList();
// return resultClusterModified
        return Ok(resultClusterModified);
    }

    // Method for calculate distance
    private double CalculateDistance(double[] firstPoint, double[] secondPoint)
    {
        var distance = firstPoint
            .Zip(secondPoint,
                (n1, n2) => Math.Pow(n1 - n2, 2)).Sum();
        return Math.Sqrt(distance);
    }
}
