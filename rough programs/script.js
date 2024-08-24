$(document).ready(function() {
    // Hide the paragraph when "Hide" button is clicked
    $("#hide").click(function() {
        $("#text").hide();
    });

    // Show the paragraph when "Show" button is clicked
    $("#show").click(function() {
        $("#text").show();
    });

    // Change the text of the paragraph when "Change Text" button is clicked
    $("#changeText").click(function() {
        $("#text").text("The text has been changed!");
    });

    // Add the "highlight" class to the paragraph when "Highlight" button is clicked
    $("#highlight").click(function() {
        $("#text").toggleClass("highlight");
    });
});
