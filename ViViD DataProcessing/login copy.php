<?php
	$con = mysqli_connect('localhost', 'root', 'root', 'unityaccess');
	if (myseqli_connect_errno())
	{
		echo "1"; //error code #1 = connection failed
		exit();
	}

	$username = $_POST["username"];
	$password = $_Post["password"];

	$namecheckquery = "SELECT username, salt, hash FROM players WHERE username='" . $username . "';"

	$namecheck = mysqli_query($con, $namecheckquery) or die('2': "Name check query failed");

	if (mysqli_num_rows($namecheck != 1) {
		echo "5: No user with name or more than one";
		exit();
	}

	$existinginfo = mysqli_fetch_assoc($namecheck);
	$salt = $existinginfo["salt"];
	$hash = $existinginfo["hash"];

	$loginhash = crypt($password, $salt);

	if ($hash != $loginhash) {
		echo "6: Incorrect password";
		exit();
	}

	echo "0\t" . $existinginfo["score"];

	







?>