using developwithpassion.bdd.contexts;
using developwithpassion.bdd.harnesses.mbunit;
using developwithpassion.bdddoc.core;

namespace MyProject.Test
{
    public class StaticClassSpecs
    {
        public abstract class concern : observations_for_a_static_sut {}

        [Concern(typeof (StaticClass))]
        public class when_static_class_is_called_to_send_message : concern
        {
            context c = () => {};

            because b = () => 
            {
                message = StaticClass.send_message("my secrete message");
            };


            it should_return_the_same_message = () =>
            {
                message.should_be_equal_to("my secrete message");
            };

            static string message;
        }
    }

    public class StaticClass {
        static public string send_message(string message)
        {
            return message;
        }
    }
}
