<?php
session_start();
?>
<!DOCTYPE html>
<html>
    <head>
        <title>BoardGames Events</title>
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
                        <h2>MAD games is an weekend event that will introduce gamers of all levels to an
                            expertly curated selection of the most fun and interesting board games ever created.</h2>
                        <h3>
                            During the entire weekend, all visitors will have full access to our library of over 200+ unique board games.
                            From the best indie board games Kickstarted this year, to the flagships from the largest publishers in the world, we have something for everyone!<br>
                            So grab a seat and Get Rolling!!!
                        </h3>
                        <img src="images/Event Pic.jpg" style="width:60%">
                    </div>
                </div>
                <div class="column centre">
                    <div class="card">
                        <h2>There are also many fun and interactive activites planned. see Schedule</h2>
                        <img src="images/Schedule.png">
                        <h3>



                        </h3>
                    </div>
                </div>
                <div class="column centre">
                    <div class="card">
                    	<h2>The above schedule is filled with super fun and rewarding activities that visitors can take part in. Here are some examples of the activities:</h2>
                        <h3>

                        	Monopoly Tournament:<br>Do you always beat your friends in monopoly? Well do we have an tournament for you.
                        	 This isn't your average monopoly game, this tournament has something other than pride at stake. Money, all the money that you can make. In this tournament the there can only be one winner. The winner of this tournament recieves the ammount they finished the game with.<br>
                        	 <br>
                        	Everyone vs Pros:<br>
                        	In this activity every particpant will play a role in the match against the Pro. Decisions will be made by voting,They will vote on things like which gamed is played and the next move. If Everyone wins all the participants will recieve event credits<br>
                        	<br>
                        	Saving the best for last there is of course the Suprise ....

                        </h3>
                    </div>
                    <div class="column centre">
                        <div class="card">
                        <h2> Shops</h2>
                        <h3>We know that board gaming can be tiring. Beating all your friends in UNO is not a easy task, thats why we have shops placed throughout the event that sell delicuous food and refreshing drinks to keep you at peak performance </h3>
                        </div>
                    </div>
                    <div class="column centre">
                        <div class="card">
                        <h2> Loan Stands</h2>
                        <h3>Forgot your phone charger? Don't fret we've got you covered. There are Loan stands throughout the event, where you can loan common items such as phone chargers, laptop chargers, cameras and more</h3>
                        </div>
                    </div>
                    <div class="column centre">
                        <div class="card">
                        <h2> Camping</h2>
                        <h3>The activities might be over at night but that doesnt mean that the fun has to be. Grab up to 5 of your friends and come on down to your reserved camping spot. Wheter you all gather by the campfire or play cards in the tent all night long, your guaranteed to have a blast</h3>
                        </div>
                    </div>
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
