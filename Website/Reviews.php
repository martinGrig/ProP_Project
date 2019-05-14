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
                  echo  "<h3>Leave your review here!</h3>
                   <form name=\"submit-review-form\" action=\"ReviewsProcess.php\" onsubmit=\"return validateReview()\" method=\"post\">
                       <textarea name=\"comment\" rows=\"10\" cols=\"30\" placeholder=\"Your Review...\"></textarea><br>
                       <label class=\"container\">Anonymous comment
                       <input type=\"checkbox\" name=\"anonymous_checkbox\" value=\"anonymous\">
                       <span class=\"checkmark\"></span>
                       </label>
                       <input type=\"submit\" value=\"Submit\"><br>
                   </form>";
                } else {
                  echo "<h3>Only event's visitors can leave reviews!</h3>";
                }
                ?>
                </div>
                <div class="card">
                    <h3>Name</h3>
                    <h4>Comment</h4>
                    <h5>Date</h5>
                </div>
                <div class="card">
                    <h3>Name</h3>
                    <h4>Comment</h4>
                    <h5>Date</h5>
                </div>
                <div class="card">
                    <h3>Name</h3>
                    <h4>Comment</h4>
                    <h5>Date</h5>
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
