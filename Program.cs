using System;
using System.Collections.Generic;
using System.Linq;

/**
 * Author: David Martinez
 * Class: CIS 390 
 * Project: Bankers Algorithm 
 */
namespace BankersAlgo
{
    class Program
    {

        static void Main(string[] args) //main simply initilizes the 3 test cases for the program
        {
            Program exampleRun = new();
            exampleRun.testCase1();
            exampleRun.testCase2();
            exampleRun.testCase3();
        }

        public void testCase1()
        {
            simuSystem systemSnapShot = new(1, 5, 2, 0); //set initial system resources available

            /**
             * Initial Creation of the 5 processes in the system snapshot with currently allocated ABCD resources and their
             * corrosponding MAX resource allocation amount
             */
            simuPro process0 = new();
            process0.initialAllocation(0, 0, 1, 2); //sets the inital resource allocation for each process
            process0.assignMax(0, 0, 1, 2); //assigns the max need for the process
            process0.procName = "P0";

            simuPro process1 = new();
            process1.initialAllocation(1, 0, 0, 0);
            process1.assignMax(1, 7, 5, 0);
            process1.procName = "P1";

            simuPro process2 = new();
            process2.initialAllocation(1, 3, 5, 4);
            process2.assignMax(2, 3, 5, 6);
            process2.procName = "P2";

            simuPro process3 = new();
            process3.initialAllocation(0, 6, 3, 2);
            process3.assignMax(0, 6, 5, 2);
            process3.procName = "P3";

            simuPro process4 = new();
            process4.initialAllocation(0, 0, 1, 4);
            process4.assignMax(0, 6, 5, 6);
            process4.procName = "P4";


            systemSnapShot.addProcessToSystem(process0); //Adding all the processes to the simulated system snapshot
            systemSnapShot.addProcessToSystem(process1);
            systemSnapShot.addProcessToSystem(process2);
            systemSnapShot.addProcessToSystem(process3);
            systemSnapShot.addProcessToSystem(process4);

            systemSnapShot.determineProcessNeed();
            systemSnapShot.systemPrinter();
            systemSnapShot.safeLogic();
            systemSnapShot.printProcess();

        }
        public void testCase2()
        {
            simuSystem systemSnapShot = new(0, 3, 0, 1);

            simuPro process0 = new();
            process0.initialAllocation(3, 0, 1, 4);
            process0.assignMax(5, 1, 1, 7);
            process0.procName = "P0";

            simuPro process1 = new();
            process1.initialAllocation(2, 2, 1, 0);
            process1.assignMax(3, 2, 1, 1);
            process1.procName = "P1";

            simuPro process2 = new();
            process2.initialAllocation(3, 1, 2, 1);
            process2.assignMax(3, 3, 2, 1);
            process2.procName = "P2";

            simuPro process3 = new();
            process3.initialAllocation(0, 5, 1, 0);
            process3.assignMax(4, 6, 1, 2);
            process3.procName = "P3";

            simuPro process4 = new();
            process4.initialAllocation(4, 2, 1, 2);
            process4.assignMax(6, 3, 2, 5);
            process4.procName = "P4";


            systemSnapShot.addProcessToSystem(process0);
            systemSnapShot.addProcessToSystem(process1);
            systemSnapShot.addProcessToSystem(process2);
            systemSnapShot.addProcessToSystem(process3);
            systemSnapShot.addProcessToSystem(process4);

            systemSnapShot.determineProcessNeed();
            systemSnapShot.systemPrinter();
            systemSnapShot.safeLogic();
            systemSnapShot.printProcess();
        }
        public void testCase3()
        {
            simuSystem systemSnapShot = new(1, 0, 0, 2);

            simuPro process0 = new();
            process0.initialAllocation(3, 0, 1, 4);
            process0.assignMax(5, 1, 1, 7);
            process0.procName = "P0";

            simuPro process1 = new();
            process1.initialAllocation(2, 2, 1, 0);
            process1.assignMax(3, 2, 1, 1);
            process1.procName = "P1";

            simuPro process2 = new();
            process2.initialAllocation(3, 1, 2, 1);
            process2.assignMax(3, 3, 2, 1);
            process2.procName = "P2";

            simuPro process3 = new();
            process3.initialAllocation(0, 5, 1, 0);
            process3.assignMax(4, 6, 1, 2);
            process3.procName = "P3";

            simuPro process4 = new();
            process4.initialAllocation(4, 2, 1, 2);
            process4.assignMax(6, 3, 2, 5);
            process4.procName = "P4";


            systemSnapShot.addProcessToSystem(process0); 
            systemSnapShot.addProcessToSystem(process1);
            systemSnapShot.addProcessToSystem(process2);
            systemSnapShot.addProcessToSystem(process3);
            systemSnapShot.addProcessToSystem(process4);

            systemSnapShot.determineProcessNeed(); //gets process need
            systemSnapShot.systemPrinter(); //prints the system snapshot
            systemSnapShot.safeLogic(); // determines if the system is in a safe state and calculates the execution sequence
            systemSnapShot.printProcess(); //prints the found safe sequence
        }
    }

    public class simuSystem //class that simulates a system containing many process with a pool of shared resources that need to be allocated in a sequence to avoid deadlock
    {
        List<simuPro> processList = new List<simuPro>();
        public int resourceAvailable_A { set; get; }
        public int resourceAvailable_B {set; get; }
        public int resourceAvailable_C {set; get; }
        public int resourceAvailable_D {set; get; }
        public bool systemSafe { set; get; }

        public simuSystem(int iniA, int iniB, int iniC, int iniD)
        {
            resourceAvailable_A = iniA;
            resourceAvailable_B = iniB;
            resourceAvailable_C = iniC;
            resourceAvailable_D = iniD;
        }

        public void addProcessToSystem(simuPro passedProc) //adds given process to the processlist for the system
        {
            processList.Add(passedProc);
        }
        public void determineProcessNeed()
        {
            foreach(simuPro curProc in processList) //traverse the list of processes to determine need
            {
                curProc.resourceNeed_A = (curProc.resourceMax_A - curProc.resourceAllocated_A);
                curProc.resourceNeed_B = (curProc.resourceMax_B - curProc.resourceAllocated_B);
                curProc.resourceNeed_C = (curProc.resourceMax_C - curProc.resourceAllocated_C);
                curProc.resourceNeed_D = (curProc.resourceMax_D - curProc.resourceAllocated_D);

            }
        }

        public void safeLogic()
        {
            int sequenceTracker = 0; //keeps track of the order the processes need to complete in
            bool runFlag = true; //if the loop should contine to iterate over the list
            systemSafe = true; //defaults to true and is changed if a deadlock is encountered

            while (runFlag == true)
            {
                bool changeMade = false; 

                foreach (simuPro curProc in processList)
                {
                    if (curProc.finishedFlag == true)
                    {
                        continue;
                    }

                    /**
                     * main logic section, determines whether or not the system has available resources to complete a give process, goes sequentially down the list 
                     * of processes and iterates over the list until all have been finished or skipped due to deadlock 
                     */
                    if (curProc.resourceNeed_A <= resourceAvailable_A && curProc.resourceNeed_B <= resourceAvailable_B && curProc.resourceNeed_C <= resourceAvailable_C
                        && curProc.resourceNeed_D <= resourceAvailable_D)
                    {
                        curProc.finishedFlag = true;
                        curProc.sequencePostion = sequenceTracker;
                        sequenceTracker++;
                        freeResources(curProc); //returns resources to pool
                        changeMade = true; 
                    }
                }
                if(changeMade == false) //Checks to see if any changes were made, if a loop goes through with no changes, then it is either finished or in a deadlock
                {
                    runFlag = false;
                    break;
                }
            }
            
            foreach(simuPro procCheck in processList) //Checking to see if each simulated process has completed and if the system is a safe state
            {
                
                if(procCheck.finishedFlag == true)
                {
                    continue;
                }
                else if(procCheck.finishedFlag == false)
                {
                    systemSafe = false;
                }
            }
        }
        public void printProcess() //Prints the sequence of processes that was determined to be safe 

        {
          
            if(systemSafe == true)
            {
                Console.WriteLine("System Status: Currently In A Safe State");
            }
            else if(systemSafe == false)
            {
                Console.WriteLine("System Status: System Unsafe, Deadlock");
            }

            processList.Sort((x, y) => x.sequencePostion.CompareTo(y.sequencePostion)); //sorted by sequence number 

            Console.Write("Process Resource Allocation Sequence: ");
            if (systemSafe == true) //a sequence only exists if the system is safe, otherwise this conditon fails and the else is executed instead
            {
                foreach (simuPro printProc in processList)
                {
                    if (printProc == processList.Last()) //linq function to determine if another pointing arrow '->' should be printed 
                    {
                        Console.Write(printProc.procName + "\n\n");
                    }
                    else
                    {
                        Console.Write(printProc.procName + " -> ");
                    }
                }
            }
            else if(systemSafe == false)
            {
                Console.Write("N/A - No safe sequence possible \n\n");
            }
        }
        public void freeResources(simuPro handedProc) //returns the resources from the completed process back to the system to apply to other processes
        {
            this.resourceAvailable_A = this.resourceAvailable_A + handedProc.resourceAllocated_A;
            this.resourceAvailable_B = this.resourceAvailable_B + handedProc.resourceAllocated_B;
            this.resourceAvailable_C = this.resourceAvailable_C + handedProc.resourceAllocated_C;
            this.resourceAvailable_D = this.resourceAvailable_D + handedProc.resourceAllocated_D;
        }

        public void systemPrinter() //Prints a snapshot of the current system, includes all processes, their resource need, allocated amount, max amount and the system available resources
        {
            Console.WriteLine("CURRENT SYSTEM SNAPSHOT");
            Console.WriteLine("======================================");
            Console.WriteLine("        Allocated  Max       Need ");
            Console.WriteLine("NAME    A B C D    A B C D   A B C D");
            foreach(simuPro printProc in processList)
            {
                Console.WriteLine("{0}      {1} {2} {3} {4}    {5} {6} {7} {8}   {9} {10} {11} {12}",printProc.procName,printProc.resourceAllocated_A, printProc.resourceAllocated_B,
                    printProc.resourceAllocated_C, printProc.resourceAllocated_D,printProc.resourceMax_A, printProc.resourceMax_B, printProc.resourceMax_C, printProc.resourceMax_D,
                    printProc.resourceNeed_A, printProc.resourceNeed_B, printProc.resourceNeed_C, printProc.resourceNeed_D);
            }
            Console.WriteLine("======================================");
            Console.WriteLine("Available System Resources: A: {0} B: {1} C: {2} D: {3}", resourceAvailable_A, resourceAvailable_B, resourceAvailable_C, resourceAvailable_D);

        }
    }
    public class simuPro //utility class that holds all the properties of the process in the algorithm
    {
        public string procName { set; get; }
        public int resourceNeed_A { set; get; }
        public int resourceNeed_B { set; get; }
        public int resourceNeed_C { set; get; }
        public int resourceNeed_D { set; get; }

        public int resourceAllocated_A { set; get; }
        public int resourceAllocated_B { set; get; }
        public int resourceAllocated_C { set; get; }
        public int resourceAllocated_D { set; get; }

        public int resourceMax_A { set; get; }
        public int resourceMax_B { set; get; }
        public int resourceMax_C { set; get; }
        public int resourceMax_D { set; get; }

        public bool finishedFlag;
        public int sequencePostion;

        public simuPro()
        {
            finishedFlag = false; //all process start unfinished
            sequencePostion = 0; // all process initilized to zero 
        }
        public void initialAllocation(int iniA, int iniB, int iniC, int iniD)
        {
            this.resourceAllocated_A = iniA;
            this.resourceAllocated_B = iniB;
            this.resourceAllocated_C = iniC;
            this.resourceAllocated_D = iniD;
        }
        public void assignMax(int maxA, int maxB, int maxC, int maxD)
        {
            this.resourceMax_A = maxA;
            this.resourceMax_B = maxB;
            this.resourceMax_C = maxC;
            this.resourceMax_D = maxD;
        }
    }
}
