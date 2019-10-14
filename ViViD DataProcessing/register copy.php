<?php
	$con = mysqli_connect('localhost', 'root', 'root', 'unityaccess');
	if (myseqli_connect_errno())
	{
		echo "1"; //error code #1 = connection failed
		exit();
	}

	$username = $_POST["name"];
	$password = $_Post["password"];

	$namecheckquery = "SELECT name FROM players WHERE name='" . $username . "';"

	$namecheck = mysqli_query($con, $namecheckquery) or die('2': "Name check query failed"); //error code #2, name check query failed

	if (mysqli_num_rows($namecheck) > 0) {
		echo "3"   
		exit();
	}

	$salt = "\$5\$rounds=5000\$" . "blizzardriot" . $username . "\$";
	$hash = crypt($password, $salt);

	$insertuserquery = "INSERT INTO players (username, hash, salt) VALUES ('" . $username . "','" . $hash . "','" . $salt . "');";
	mysqli_query($con, $insertuserquery) or die ("4: Insert player query failed");

	echo ("0");

?>