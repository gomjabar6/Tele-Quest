// function hitEndpoint(url) {
//   var xhttp = new XMLHttpRequest();
//   xhttp.onreadystatechange = function() {
//     //we're done making the request
//     if (this.readyState === 4) {
//       //status code was successful
//       if (this.status === 200) {
//         console.log("hit endpoint successfully");
//       } else {
//         console.log("error hitting endpoint");
//       }
//     }
//   };
//   xhttp.open("POST", url);
//   xhttp.send();
// }



window.addEventListener( "load", function () {
    function sendData() {
      const XHR = new XMLHttpRequest();
  
      // Bind the FormData object and the form element
      const FD = new FormData( form );
  
      // Define what happens on successful data submission
      XHR.addEventListener( "load", function(event) {
        alert( event.target.responseText );
      } );
  
      // Define what happens in case of error
      XHR.addEventListener( "error", function( event ) {
        alert( 'Oops! Something went wrong.' );
      } );
  
      // Set up our request
      XHR.open( "POST", "https://telequestbackend.azurewebsites.net/api/Raid/CreateRaid" );
  
      // The data sent is what the user provided in the form
      XHR.send( FD );
    }
   
    // Access the form element...
    let form = document.getElementById( "raidForm" );
  
    // ...and take over its submit event.
    form.addEventListener( "submit", function ( event ) {
      event.preventDefault();
  
      sendData();
    } );
  } );