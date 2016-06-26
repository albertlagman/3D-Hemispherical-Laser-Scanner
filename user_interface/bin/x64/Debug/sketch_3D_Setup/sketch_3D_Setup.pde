// 3D Environment Laser Scanner (Processing Part2)
// By Callan Mackay
// Published on 22/02/2016


void setup(){
  size(1000, 1000, P3D);  // make the render window 1000 x 1000 pixels and 3D capable
}

float rotmot;                // current revolution angle of the motor
float rottilt;                // current tilt angle
float rotcam = 16*PI/180;    // CHANGE THIS ACCORDINGLY the angle of the webcam where 0deg is straight forward and 90deg is looking at the laser (converted to radians)  //EDITED
float rotlaser = 0;          // same as above but for the linear laser
float rotpixX;               // angle of the current white pixel from the camera's centre view in X-direction
float rotpixY;               // angle of the current white pixel from the camera's centre view in Y-direction

float pixangleX = 70.5802/640*PI/180;  // angle between each pixel in webcam in X-direction                                                                              //EDITED
float pixangleY = 55.921868/480*PI/180;  // angle between each pixel in webcam in Y-direction                                                                            //EDITED

float[] A,B,AB;                // constants needed for line equation
float[] C,D,CD;                // constants needed for plane equation
float n,t;                     // constants needed for line/plane intersection equation
float pointX, pointY, pointZ;  // temporarily holds the X,Y,Z coordinates of a new point in 3D space

float[] drawpointX;            // holds all X coordinates of points in 3D space
float[] drawpointY;            // holds all Y coordinates of points in 3D space
float[] drawpointZ;            // holds all Z coordinates of points in 3D space

boolean runOnce = true;

int k = 0;  // counts the number of white pixels in entire scan for placing them in the "drawpoint" vectors

void draw(){
  background(0);                                  // make the background black
  perspective(PI/3.0,(float)width/height,1,500);  // adjust the near and far clipping planes to see the whole scene (last two number)
  stroke(255, 0, 0);                              // make the line red
  line(0,0,0,50,0,0);                             // make a line in the x-direction of 50
  stroke(0, 255, 0);                              // make the line green
  line(0,0,0,0,50,0);                             // make a line in the y-direction of 50
  stroke(0, 0, 255);                              // make the line blue
  line(0,0,0,0,0,50);                             // make a line in the z-direction of 50
  
  camera(50*cos(mouseX/100.0)+10, 20, -50*sin(mouseX/100.0)-30,  // X,Y,Z location of the camera (not the webcam)
        10, 0, -30,                                              // X,Y,Z location of what the camera is looking at (not the webcam)
        0.0, -1.0, 0.0);                                         // where is up for the camera (not the webcam)
  
  if(runOnce){  // only renders point cloud if a key is pressed on the keyboard
    
    String[] myString = loadStrings("data.txt");                     // save the text file to the first element of a string vector                               //EDITED
    drawpointX = new float[splitTokens(myString[0], ",").length/2];  // make the drawpointX vector the length of how many white pixels there are
    drawpointY = new float[splitTokens(myString[0], ",").length/2];  // make the drawpointY vector the length of how many white pixels there are
    drawpointZ = new float[splitTokens(myString[0], ",").length/2];  // make the drawpointZ vector the length of how many white pixels there are
    myString = splitTokens(myString[0], ";");                        // split up the large string into multiple string each containing a webcam frame
    
    k = 0;  // reset "k" to zero
    
    for(int i = 0; i < myString.length; i++){               // repeat loop for every element in "myString" (number of webcam frames)
      String[] stringPart = splitTokens(myString[i], ",");  // split "myString" up into each pixel coordinate
      rottilt = float(stringPart[0]);                        // save the motor angle from the first string element to "rotmot"
      rotmot = float(stringPart[1]);
      
      pushMatrix();             // create set of local coordinates
      rotateY(-rotmot*PI/180);  // rotate everything in Y by the motor angle
      rotateX(rottilt*PI/180);  // rotate everything in Y by the motor angle                                                                                    //TEST THIS
      
      pushMatrix();             // create another set of local coordinates
      translate(-8, 2.6, 0);     // move everything along the scanner arm to the laser position in 3D space                                                      //EDIT THIS
      rotateY(rotlaser);        // rotate everything in Y by the laser angle
      
      //Plane Equation
      C = new float[]{modelX(0,0,0), modelY(0,0,0), modelZ(0,0,0)};  // create a point on laser plane with global coordinates
      translate(10, 0, 0);                                           // move along by 10 in X-direction                                                         //EDIT THIS FOR TILT
      D = new float[]{modelX(0,0,0), modelY(0,0,0), modelZ(0,0,0)};  // create another point, 10 in X-direction away from laser plane 
      CD = new float[]{D[0]-C[0], D[1]-C[1], D[2]-C[2]};             // create a line perpindicular to plane using points "C" and "D" (NORMAL VECTOR)
      // CD[0]x + CD[1]y + CD[2]z = n
      
      popMatrix();  // close off local coordinates to return current position to the motor shaft with the motor's angle
      
      for(int j = 2; j < stringPart.length; j = j+2){      // make the loop repeat for every second element in the "stringPart" vector (repeats for every set of X-Y white pixel coordinate in a single webcam frame)  //EDITED
        rotpixX = (float(stringPart[j])-320)*pixangleX;    // measuring the white pixels angle in X-direction from the cameras centre 
        rotpixY = (240-float(stringPart[j+1]))*pixangleY;  // measuring the white pixels angle in Y-direction from the cameras centre
        
        pushMatrix();           // move into another set of local coordinates
        translate(6, 2, 0);  // move along the arm from the motor shaft to the webcam's centre                                                               //EDIT THIS
        rotateY(rotcam);        // rotate to the webcam's angle
        rotateX(rotpixY);       // rotate the imaginary line by "rotpixY" in the X-axis to align with current white pixel
        rotateY(rotpixX);       // rotate the imaginary line by "rotpixX" in the Y-axis to align with current white pixel
        
        //Line Equation
        A = new float[]{modelX(0,0,0), modelY(0,0,0), modelZ(0,0,0)};  // create a point on webcam's centre with global coordinates
        translate(0,0,10);                                             // move along by 10 in Z-direction
        B = new float[]{modelX(0,0,0), modelY(0,0,0), modelZ(0,0,0)};  // create another point, 10 in Z-direction out in front of the webcam, along the imaginary line
        AB = new float[]{B[0]-A[0], B[1]-A[1], B[2]-A[2]};             // create the line from the webcam using points "A" and "B" (PROJECTION LINE)
        
        popMatrix();  // close off local coordinates to return current position to the motor shaft with the motor's angle
        
        // Intersection of Line and Plane
        t = (n - CD[0]*A[0] - CD[1]*A[1] - CD[2]*A[2])
        /   (CD[0]*AB[0] + CD[1]*AB[1] + CD[2]*AB[2]);  // Equation for the laser plane rearranged with equations for projection line substituted in to find constant "t"
        pointX = A[0] + AB[0]*t;                        // Using "t" in line equation to find X-coordinate of intersection
        pointY = A[1] + AB[1]*t;                        // Using "t" in line equation to find X-coordinate of intersection
        pointZ = A[2] + AB[2]*t;                        // Using "t" in line equation to find X-coordinate of intersection
        
        drawpointX[k] = (pointX);  // inserting temporary X-coordinate into the X-coordinate vector to hold all data points
        drawpointY[k] = (pointY);  // inserting temporary Y-coordinate into the Y-coordinate vector to hold all data points
        drawpointZ[k] = (pointZ);  // inserting temporary Z-coordinate into the Z-coordinate vector to hold all data points
        k++;                       // increment "k" for moving onto the next white pixel i.e. data point
      }
      popMatrix();  // close off local coordinates to return current position to (0,0,0) in global space
    }
    runOnce = false;
  }
  
  if(k > 0){                                               // check to see if any data points were created
    for (int i = 0; i < drawpointX.length; i++){           // repeat loop for every data point
      stroke(255);                                         // make the data point white
      point(drawpointX[i], drawpointY[i], drawpointZ[i]);  // draw the point cloud using the "drawpointX,Y,Z" vectors
    }
  }
}