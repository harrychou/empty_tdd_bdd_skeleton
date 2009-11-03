using System;
using developwithpassion.bdd.contexts;
 using developwithpassion.bdd.harnesses.mbunit;
 using developwithpassion.bdddoc.core;

namespace SampleProject.Test
 {   
     public class MyClassSpecs
     {
         public abstract class concern : observations_for_a_sut_without_a_contract<MyClass>
         {
         }

         [Concern(typeof(MyClass))]
         public class when_adding_2_plus_3 : concern
         {
             because b = () =>
             {
                 Console.WriteLine();
                 message = sut.SendMessage("message");
             };

        
             it should_equals_to_5 = () =>
             {
                 message.should_be_equal_to("message");
             };

             static string message;
         }
     }

     public class MyClass {
         public string SendMessage(string message)
         {
             return message;
         }
     }
 }
