<?php
session_start();
?>
<!DOCTYPE html>
<html>
    <head>
        <title>BoardGames Camp Spot</title>
        <link href="styles/styles.css" type="text/css" rel="stylesheet"></link>
        <script src="js/js.js"></script>
        <meta charset="UTF-8">
    </head>
    <body>
        <div class="header">
        </div>
        <div class="topnav">
            <a href="Home.php">Home</a>
            <a href="Events.php">Events</a>
            <a href="ContactUs.php">Contact us</a>
            <a href="Reviews.php">Reviews</a>
            <?php
            if (isset($_SESSION['loggedin'])){
              echo "<a href=\"LogOut.php\" style=\"float:right\">Log Out</a>";
               echo "<a href=\"Profile.php\" style=\"float:right\">Profile</a>";
             } else {
              echo "<a href=\"LogIn.php\" style=\"float:right\">Log in</a>";
             }
             ?>
        </div>
        <div class="row">
            <div class="column centre ">
              <div class="card">
                <h2>Reserve A Camp Spot</h2>
                <div>
                    <img src="images/spots.png" style="width:50%">
                </div>
                <form name="Camp-form"  action="CampProcess.php" method="post">
                <h4>

                <?php
                $username = 'dbi410102';
                $password = 'prop17';
                $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);

                $sql = "select * from campspot where reservedPlaces < capacity";
                $statement = $con->prepare($sql);
                $statement->execute();
                $result = $statement->fetchAll();
                echo "<select name=\"spots\" onchange=\"campchanged()\">";
                for ($i = 0; $i < sizeof($result); $i++) {
                  echo "<option value=\"";
                  echo $result[$i]['name'];
                  echo "\">SPOT ";
                  echo $result[$i]['name'];
                  echo "</option>";
                }
                echo "</select>";

                ?>

                </h4>
                    <select name="people">
                        <option value="1">1 Person</option>
                        <option value="2">2 Persons</option>
                        <option value="3">3 Persons</option>
                        <option value="4">4 Persons</option>
                        <option value="5">5 Persons</option>
                        <option value="6">6 Persons</option>
                    </select><br>
                    <label class="container">Pay now
                    <input type="checkbox" name="paynow_checkbox" value="paynow">
                    <span class="checkmark"></span>
                    </label>
                    <input type="submit" value="Accept">
                </form>
              </div>
            </div>
        </div>
        <div class="row">
            <div class="footer">
                <h2>
                    ADDRESS
                </h2>
                <h3>
                    <br>
                    Eindhoven, Netherlands
                </h3>
            </div>
            <div class="footer">
                <h2>
                    CONTACT
                </h2>
                <h3>
                    <br>
                    info@mysite.com<br>
                    Tel: 123-456-7890
                </h3>
            </div>
            <div class="footer">
                <h2>
                    BoardGames
                </h2>
                <h3>
                    <br>
                    © 2019 by BoardGames
                </h3>
            </div>
            <div class="footer">
                <h2>
                    WE ACCEPT
                </h2>
                <h3>
                    <img src="images/weaccept.png" width="200" height="75">
                </h3>
            </div>
        </div>
    </body>
</html>
