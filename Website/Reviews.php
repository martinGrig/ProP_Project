<?php
session_start();
?>
<!DOCTYPE html>
<html>
    <head>
        <title>BoardGames Reviews</title>
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
            <div class="column centre">
              <div class="card">
                    <h2>Reviews</h2>
                <?php
                if (isset($_SESSION['loggedin']) && isset($_SESSION['currentUser']['ticketNr'])) {
                  if ($_SESSION['currentUser']['hasReview'] == 1) {
                  echo "<h4>(You have already left your review)</h4>";
                  } else {
                  echo  "<h3>Leave your review here!</h3>
                   <form name=\"submit-review-form\" action=\"ReviewsProcess.php\" onsubmit=\"return validateReview()\" method=\"post\">
                       <textarea name=\"comment\" rows=\"10\" cols=\"30\" placeholder=\"Your Review...\"></textarea><br>
                       <label class=\"container\">Anonymous comment
                       <input type=\"checkbox\" name=\"anonymous_checkbox\" value=\"anonymous\">
                       <span class=\"checkmark\"></span>
                       </label>
                       <input type=\"submit\" value=\"Submit\"><br>
                   </form>";
                 }
                } else {
                  echo "<h3>Only event's visitors can leave reviews!</h3>";
                }
                ?>
                </div>
                  <?php
                  $username = 'dbi410102';
                  $password = 'prop17';
                  $con = new PDO('mysql:host=studmysql01.fhict.local;dbname=dbi410102', $username, $password);
                  $sql = "select * from review";
                  $statement = $con->query($sql);
                  $statement->execute();
                  $result = $statement->fetchAll();
                  for ($i = 0; $i < sizeof($result); $i++) {
                    echo "<div class=\"card\">";
                    echo "<h3>" . $result[$i]['name'] . "</h3>";
                    echo "<h4>" . $result[$i]['reviewText'] . "</h4>";
                    echo "<h5>" . $result[$i]['datetime'] . "</h5>";
                    echo "</div>";
                  }
                  ?>
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
