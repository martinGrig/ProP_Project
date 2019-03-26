var slideIndex = 0;
showSlides();

function showSlides() {
  var i;
  var slides = document.getElementsByClassName("mySlides");
  var dots = document.getElementsByClassName("dot");
  for (i = 0; i < slides.length; i++) {
    slides[i].style.display = "none";
  }
  slideIndex++;
  if (slideIndex > slides.length) {slideIndex = 1}
  for (i = 0; i < dots.length; i++) {
    dots[i].className = dots[i].className.replace(" active", "");
  }
  slides[slideIndex-1].style.display = "block";
  dots[slideIndex-1].className += " active";
  setTimeout(showSlides, 4000); // Change image every 2 seconds
}

function validateContactUs() {
  var name = document.forms["contact-input-form"]["firstname"].value;
  var email = document.forms["contact-input-form"]["email"].value;
  var number = document.forms["contact-input-form"]["number"].value;
  var comment = document.forms["contact-input-form"]["comment"].value;

  if (name == "") {
    alert("Name must be filled out");
    return false;
  }
  if (email == "") {
    alert("Email must be filled out");
    return false;
  }

  if (number == "") {
    alert("Phone number must be filled out");
    return false;
  }

  if (comment == "") {
    alert("Your feedback can't be empty");
    return false;
  }
}

function validateReview() {
  var review = document.forms["submit-review-form"]["comment"].value;

  if (review == "") {
    alert("Review can't be empty");
    return false;
  }
}

function validateRegistration() {
  var firstname = document.forms["registration-form"]["fname"].value;
  var lastname = document.forms["registration-form"]["lname"].value;
  var email = document.forms["registration-form"]["email"].value;
  var password = document.forms["registration-form"]["number"].value;
  var iban = document.forms["registration-form"]["iban"].value;


  if (firstname == "") {
    alert("First name must be filled out");
    return false;
  }
  if (lastname == "") {
    alert("Last name must be filled out");
    return false;
  }

  if (email == "") {
    alert("Email number must be filled out");
    return false;
  }

  if (password == "") {
    alert("Password can't be empty");
    return false;
  }

  if (iban == "") {
    alert("IBAN can't be empty");
    return false;
  }
}

function validateLogin() {
  var number = document.forms["login-form"]["number"].value;
  var password = document.forms["login-form"]["password"].value;

  if (number == "") {
    alert("Ticket number must be filled out");
    return false;
  }
  if (password == "") {
    alert("Password must be filled out");
    return false;
  }
}
