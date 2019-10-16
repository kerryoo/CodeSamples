<?php 
	$con = mysqli_connect('localhost', 'root', 'root', 'unityaccess');
	if (myseqli_connect_errno())
	{
		echo "1"; //error code #1 = connection failed
		exit();
	}

	$username = $POST["username"];
	$level = $POST["level"];
	$specialabilitycount = $_POST["specialabilitycount"];
	$timetocompletelevel = $_POST["timetocompletelevel"];
	$deathcount = $_POST["deathcount"];
	$playerpositions = $_POST["playerpositions"];

	$insertuserquery = "INSERT INTO playerdata (username, level, timetocompletelevel, deathcount, specialabilitycount, playerpositions) 
	VALUES ('" . $username . "','" . $level . "','" . $specialabilitycount . "','" . 
		$timetocompletelevel . "','" . $deathcount  . "'.'" . $playerpositions . ");";

	mysqli_query($con, $insertuserquery) or die ("4: Insert player data query failed");

	echo ("0");

 ?>