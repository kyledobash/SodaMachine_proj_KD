using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class Coin
    {
        //Member Variables (Has A)
        // Member Variable = Field
        // A field is what a Class HAS
        // It could be accessible (public) to the outside world, or protected/private
        protected double value;
        public string Name;

        //Properties
        // Properties EXTEND Fields
        // The Superhero Identity for the Alter Ego - the mask presented to the outside world
        public double Value
        {
            // GET + SET
            // GET = READ
            // SET = WRITE
            get
            {
                return value;
            }
           
        }
        //Constructor (Spawner)

        //Member Methods (Can Do)
    }
}
