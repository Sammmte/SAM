using System;
using System.Collections.Generic;
using SAM.StateMachine;

namespace TestProject
{
    class Program
    {
        static void Main(string[] args)
        {
            StateMachine stateMachine = new StateMachine();

            State1 s1 = new State1();
            State2 s2 = new State2();
            State3 s3 = new State3();

            stateMachine.Add(s1);
            stateMachine.Add(s2);
            stateMachine.Add(s3);

            stateMachine.SwitchTo(s1);

            string input = null;

            while(input != "quit")
            {
                stateMachine.Update();

                if(stateMachine.Current == s1)
                {
                    stateMachine.SwitchTo(s2);
                }
                else if(stateMachine.Current == s2)
                {
                    stateMachine.SwitchTo(s3);
                }
                else
                {
                    stateMachine.SwitchTo(s1);
                }

                input = Console.ReadLine();
            }
        }
    }
}
