# assignment-4-alpartun




## Table of Contents



1. [ Task ](#task)
2. [ K-Means Algorithm ](#kmean)
3. [My Approach](#my)
4. [ Postgre SQL ](#postg)
- 4.1[ Container Table ](#conttab)
5. [ Postman ](#post)


<a name="task"></a>
## 1.Task

<img width="510" alt="taskPhoto1" src="https://user-images.githubusercontent.com/36279321/188620609-53ad3e61-98ba-40e0-9d93-4f3efff60243.png">

<img width="508" alt="taskPhoto2" src="https://user-images.githubusercontent.com/36279321/188620783-16d38687-c3a5-426d-af77-b12cda61f19f.png">

<a name="kmean"></a>
## 2.K-Means Algorithm

K-Means Clustering is an Unsupervised Learning algorithm, which groups the unlabeled dataset into different clusters.
It allows us to cluster the data into different groups and a convenient way to discover the categories of groups in the unlabeled dataset on its own without the need for any training.

It is a centroid-based algorithm, where each cluster is associated with a centroid. The main aim of this algorithm is to minimize the sum of distances between the data point and their corresponding clusters.

 The k-means clustering algorithm mainly performs two tasks:

* Determines the best value for K center points or centroids by an iterative process.

* Assigns each data point to its closest k-center. Those data points which are near to the particular k-center, create a cluster.

<img width="586" alt="beforeAfterKMeans" src="https://user-images.githubusercontent.com/36279321/188622284-e9e29cbd-6027-4f2c-9cc6-4269dcd4034d.png">

<a name="my"></a>
## 3.My Approach

Points are placed randomly in figures.

* Step 1 ->

<img width="1033" alt="Ekran Resmi 2022-09-06 14 23 39" src="https://user-images.githubusercontent.com/36279321/188641752-b5f54102-aeca-43ed-ad70-d78a19ff2598.png">

* Step 2 ->

<img width="1014" alt="Ekran Resmi 2022-09-06 14 27 04" src="https://user-images.githubusercontent.com/36279321/188641830-77f03696-5cfa-4699-8f24-c26f1116a0d9.png">

Select first n element and seperate n clusters.(n = color in here for better understand)

* Step 3 ->

<img width="1037" alt="Ekran Resmi 2022-09-06 14 29 21" src="https://user-images.githubusercontent.com/36279321/188642197-79a4a8fc-22a5-47d3-b06e-b7202e0af007.png">

For all remained points that are not grouped, we give randomly cluster range(0,n)

* Step 4 ->

<img width="1013" alt="Ekran Resmi 2022-09-06 14 41 22" src="https://user-images.githubusercontent.com/36279321/188642660-6c6d7d8b-e434-44dd-a449-f6d4f788e0ab.png">

Calculate average of all same colored points x and y values then assign to centroid coordinate. 

* Step 5 ->

<img width="1018" alt="Ekran Resmi 2022-09-06 15 48 08" src="https://user-images.githubusercontent.com/36279321/188643431-b827e3ac-6ef9-44b5-8c55-1aba965169de.png">

Calculate distance between point and centroids. Then select the nearest one. This step applied for each points.

<img width="952" alt="Ekran Resmi 2022-09-06 15 48 56" src="https://user-images.githubusercontent.com/36279321/188643987-4c9223e0-0a16-4e5f-b516-2682bc75901a.png">
(After selecting nearest one, the point color changes.)

<img width="955" alt="Ekran Resmi 2022-09-06 15 50 48" src="https://user-images.githubusercontent.com/36279321/188644328-2880b533-31c0-47e2-bf02-c9615ffcd027.png">

<img width="807" alt="Ekran Resmi 2022-09-06 15 53 20" src="https://user-images.githubusercontent.com/36279321/188644378-643f65ee-afb2-4ec5-83a3-e43fc5a9e3e3.png">

Take every same colored points and get average of that points then move centroids to that coordinates.


* Step 6 ->

<img width="1023" alt="Ekran Resmi 2022-09-06 15 59 27" src="https://user-images.githubusercontent.com/36279321/188644425-7180c86d-fe37-4289-a5fe-045df080710f.png">

Then repeat step 5 until centroid coordinates do not change.

<a name="postg"></a>
## 4.Postgre SQL

<a name="conttab"></a>
### 4.1 Container Table

<img width="570" alt="Ekran Resmi 2022-09-06 16 23 36" src="https://user-images.githubusercontent.com/36279321/188646504-5b582990-86e1-4e0f-96ec-03e1fbcbbd4d.png">

<a name="post"></a>
## 5.Postman

<img width="1030" alt="Ekran Resmi 2022-09-06 16 25 22" src="https://user-images.githubusercontent.com/36279321/188646847-e48e9194-ddd2-4463-94ec-55a1f92fc8ef.png">

<img width="964" alt="Ekran Resmi 2022-09-06 16 35 27" src="https://user-images.githubusercontent.com/36279321/188657397-a69f58b4-d3f7-4362-b383-ab22e1878af8.png">


<img width="920" alt="Ekran Resmi 2022-09-06 16 35 49" src="https://user-images.githubusercontent.com/36279321/188657422-2add64b5-f8b0-46d8-bb53-e25cf1331995.png">

<img width="978" alt="Ekran Resmi 2022-09-06 16 36 10" src="https://user-images.githubusercontent.com/36279321/188657489-a811c50d-fd0d-4196-9016-71584704a066.png">

