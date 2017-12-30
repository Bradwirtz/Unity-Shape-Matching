FILE DESCRIPTIONS:

-Basic_Deformer.cs:     Code that takes collider inputs and translates the force into an initial movement of the particles. Should be
                        attached to GameObject intended to undergo deformation. higher dampen values decreases the severity of deformation. 
                        Min force establishes the minimum force necessary to cause a deformation. Cooldown frames determines how many frames 
                        after a deformation there are before another can occur. particles_container is a field that must be filled before
                        running the game, it should be a parent gameobject whose children are the points that should be moved on collision.

-Capsule_Controller.cs: Positions capsule colliders to form a collision latice, updates positions of colliders when deformation occurs

-Latice_Creator.cs:     Takes a 3D mesh as input and translates the veritices into particles and edges into collider rods. Should be 
                        attached to the GameObject containing the desired mesh. Once latice has been created it can be saved as a permanent 
                        GameObject.

-MatrixFunctions.cs:    Contains all standard matrix computations

-Rotation_Test.cs:      Runs basic testing for shape_matching code. Given a mesh and a marker Gameobject, the test places markers at each
                        vertex of the mesh and applies a random rotation along with controlably random movement for each point for the 
                        shapematching engine to approximate. In its current state the vertices are commited to memory after being manipulated
                        rather than filtered through the shader, which would be the ideal method. Duplicate should be filled with the parent   
                        object desired for the markers which are generated at runtime. Randomess dictates the amaount points are randomly 
                        moved in each direction with each test.

-ShapeMatching.cs:      Object that should be instantiated for each cluster of a deformable object. Initial input is the parent object housing 
                        particles in their original positions. When particles are moved either generate_rotation_matrix or generate_linear_matrix
                        should be called to generate the matrix for the vertices to be filtered through.




