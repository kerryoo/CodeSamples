<?php 
	$con = mysqli_connect('localhost', 'root', 'root', 'unityaccess');
	if (myseqli_connect_errno())
	{
		echo "1"; //error code #1 = connection failed
		exit();
	}

	$username = $POST["username"];
	$timetocompletelevel = $_POST["timetocompletelevel"];
	$deathcount = $_POST["deathcount"];
	$specialabilitycount = $_POST["specialabilitycount"];
	$playerpositions = $_POST["playerpositions"];

	$insertuserquery = "INSERT INTO playerdata (username, timetocompletelevel, deathcount, specialabilitycount, playerpositions) 
	VALUES ('" . $username . "','" . $timetocompletelevel . "','" . $deathcount . "','" . $specialabilitycount . "'.'" . $playerpositions . ");";
	mysqli_query($con, $insertuserquery) or die ("4: Insert player data query failed");

	$updatequery = "UPDATE playerdata SET "

 ?>