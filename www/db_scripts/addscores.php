<?php
$hostname = 'localhost';
$username = 'root';
$password = '';
$database = 'collect_object';
$secretKey = "mySecretKey";
 
try 
{
	$dbh = new PDO('mysql:host='. $hostname .';dbname='. $database, 
           $username, $password);
} 
catch(PDOException $e) 
{
	echo '<h1>An error has ocurred.</h1><pre>', $e->getMessage() 
            ,'</pre>';
}

$hash = $_GET['hash'];
$realHash = hash('sha256', $_GET['name'] . $_GET['mode'] . $secretKey);
	
if($realHash == $hash) 
{ 
	$sth = $dbh->prepare('INSERT INTO scores VALUES (:name
            , :mode, 0 )');
	try 
	{
		$sth->bindParam(':name', $_GET['name'], 
                  PDO::PARAM_STR);
		$sth->bindParam(':mode', $_GET['mode'], 
                  PDO::PARAM_INT);
		$sth->execute();
	}
	catch(Exception $e) 
	{
		echo '<h1>An error has ocurred.</h1><pre>', 
                 $e->getMessage() ,'</pre>';
	}
}

?>