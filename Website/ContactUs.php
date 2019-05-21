<?php
session_start();
?>
<!DOCTYPE html>
<html>
    <head>
        <title>BoardGames Contact Us</title>
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
                <h2>About us</h2>
                <h3>We are MAD Projects, a small but motivated group specializing in ICT.
                    We believe passionately in great bargains and excellent service, which is why we commit ourselves to giving you the best of both.
                    <br>
                    If you’re looking for something new, fun and interesting, you’re in the right place. We strive to be industrious and innovative,
                    offering our visitors something they want, putting their desires at the top of our priority list.</h3>
                <h2>Contact Us</h2>
                <h3>If you have any questions, please contact us by telephone or email and we'll get back to you as soon as possible.
                    We look forward to hearing from you.</h3>
                <form name="contact-input-form" action="ContactUsProcess.php" onsubmit="return validateContactUs()" method="post">
                    <input name="firstname" type="text" placeholder="First Name...">
                    <input type="email" name="email" placeholder="Your E-mail...">
                    <input name="number" type="tel" placeholder="Your Phone Number..."><br>
                    <textarea name="comment" rows="10" cols="30" placeholder="Your feedback..."></textarea><br>
                    <input type="submit" value="Submit"><br>
                </form>
                <iframe
                width=76% height="200" id="gmap_canvas" src="https://maps.google.com/maps?q=eindhoven%205612jp&t=&z=13&ie=UTF8&iwloc=&output=embed" frameborder="0" scrolling="no" marginheight="0" marginwidth="0">
                </iframe>
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
