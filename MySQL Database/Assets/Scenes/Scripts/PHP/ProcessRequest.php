<?PHP

$request = $_POST['request'];
$connection = $_POST['connection'];
$username = $_POST['username'];
$password = $_POST['password'];
$database = $_POST['database'];
//echo $request; DEBUG
$mysqli = new mysqli($connection, $username, $password, $database); //"localhost", "root", "", "testdb"
if (!$mysqli)
	die("Could Not Connect\n");
else{
	if ($mysqli -> connect_errno) {
		die("Error, connection error");
	}
	if ($result = $mysqli -> query($request)) { //$request "SELECT * FROM first WHERE id=101"
	//echo "Returned rows are: " . $result -> num_rows;
	if (mysqli_num_rows($result)==0) 
	{ 
		echo "No results found\n";
	}
	$num_coulmns = mysqli_field_count($mysqli);;
	 while($row = mysqli_fetch_array($result))
     {
        //echo implode(", ", $row);
		for($i = 0; $i < $num_coulmns; $i++)
		{
			echo $row[$i];
			echo ", ";
		}
		echo "\n";
     } 
	 
	// Free result set
	$result -> free_result();
	}
	else
		echo "Invalid, check input"; 
}
?>