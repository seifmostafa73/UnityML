# UnityML

## Simulation

### Implementation
I tried to make this project as generic as possible to avoid repeating code. and to make it reusable in different robotic projects .
I've implmented 4 main classes that I'll discuss briefly.

#### Qlearining : 
you can find the actual algorith implmentation here , I used only Numsharp to implement the algortihm.

#### ServoHeadController : 
a simple wrapper class that uses articulation bodies to emulate the simple functions of Servos in embbeded systems.

#### Enivronment : 
this is an abstract class that Qlearning interacts with , so it's Independent on user's environment implementation.

#### CrawlerEnvironment : 
here you can find the actual environment implmentation (Actions , states , Functions , Qtable etc.)

### final system testing without Qlearn applied. 

https://user-images.githubusercontent.com/55613060/175782509-234cc2de-9925-4d48-a7f9-fb3b284c003b.mp4

### Qlearning parameter tweeking 
I'm glad to say that using this approach to the project has saved me countless hours of training time to reach the optimal values for the parameters.

I first started with default paramters for the Qlearn , and tested the whole system.

https://user-images.githubusercontent.com/55613060/175782583-8336f175-53bc-4ae8-ba46-5241ddfe6858.mp4

then after training 2 full episdoes ( about 1000 steps tried) i found out that the robot had and idea of what the optimal action was but still explored better ones . 
so i repeated the test , this time training 10 models at once with different paramters 

https://user-images.githubusercontent.com/55613060/175782683-4b9b8822-7e86-427b-9121-df84fd3cfccb.mp4

finally i settled with the best 3 paramters set values , and made them compete for the best , these were the rsults after 30 min of training 

https://user-images.githubusercontent.com/55613060/175782753-7b3fe07b-32e4-4573-8d24-771bf3d5921c.mp4

![0016647610_10](https://user-images.githubusercontent.com/55613060/175782821-d5d5d988-1b8c-469a-a159-705e3e1ef80f.jpg)

https://user-images.githubusercontent.com/55613060/175782833-ba9e5bd8-0ca3-43d5-b4dc-be0143c89b33.mp4



## Hardware results

### Simulation vs hardware
I began this project to be used only as a basis to understand the algorithm in depth and to cut training time and paramters tuning , but ended up actually using the exact same code , down to the delay values ! 
while i only tweaked little physics paramters in the settings , the simulation yielded a pretty accurate results, this can be seen in the comparison between the C and C# implemntation (yeah I know C# has #defines but my brain couldn't live in a world where that is true )

![image](https://user-images.githubusercontent.com/55613060/175784000-e1bf9d34-abc0-4aff-be46-8eb30090023e.png)


