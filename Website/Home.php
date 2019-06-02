<?php
session_start();
?>

<!DOCTYPE html>
<html>
    <head>
        <title>BoardGames Home</title>
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
                    <h1>MAD Games</h1>
                    <h2>Discover a new breed of Board Games</h2>
                    <div class="mySlides fade">
                    <img src="images/a.jpg" style="width:60%">
                    </div>

                    <div class="mySlides fade">
                    <img src="images/b.jpg" style="width:60%">
                    </div>

                    <div class="mySlides fade">
                    <img src="images/c.jpg" style="width:60%">
                    </div>

                    <br>

                    <div style="text-align:center">
                    <span class="dot"></span>
                    <span class="dot"></span>
                    <span class="dot"></span>
                    </div>

                    <h2>Whether you are only curious or a tried-and-true hardcore gamer, you can join out event this summer.</h2>

                    <h2>July 5 - 7 2019,Eindhoven</h2>

                    <?php
                    if (isset($_SESSION['loggedin'])){
                        if (!isset($_SESSION['currentUser']['ticketNr'])) {
                            echo "  <form action=\"Buy.php\">
                            <input type=\"submit\" value=\"Buy A Ticket\"/>
                            </form>";
                          }
                    } else {
                    echo "  <form action=\"Register.php\">
                            <input type=\"submit\" value=\"Buy A Ticket\"/>
                            </form>";
                    }
                    ?>

                </div>
            </div>
        </div>
        <div class="footer">
            <h2>
                ADDRESS
            </h2>
            <h3>
                <br>
                Eindhoven, Netherlands
            </h3>
        </div>
        <div class="row">
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
        <script src="js/js.js"></script>
    </body>
</html>
