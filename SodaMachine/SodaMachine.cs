using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SodaMachine
{
    class SodaMachine
    {
        //Member Variables (Has A)
        private List<Coin> _register;
        private List<Can> _inventory;

        //Constructor (Spawner)
        public SodaMachine()
        {
            _register = new List<Coin>();
            _inventory = new List<Can>();
            FillInventory();
            FillRegister();
        }

        //Member Methods (Can Do)

        //A method to fill the sodamachines register with coin objects.
        public void FillRegister()
        {
            for (int i = 0; i < 20; i++)
            {
                _register.Add(new Quarter());
            }
            for (int i = 0; i < 10; i++)
            {
                _register.Add(new Dime());
            }
            for (int i = 0; i < 20; i++)
            {
                _register.Add(new Nickel());
            }
            for (int i = 0; i < 50; i++)
            {
                _register.Add(new Penny());
            }
        }
        //A method to fill the sodamachines inventory with soda can objects.
        public void FillInventory()
        {
            for (int i = 0; i < 10; i++)
            {
                _inventory.Add(new RootBeer());
            }
            for (int i = 0; i < 15; i++)
            {
                _inventory.Add(new OrangeSoda());
            }
            for (int i = 0; i < 20; i++)
            {
                _inventory.Add(new Cola());
            }
        }
        //Method to be called to start a transaction.
        //Takes in a customer which can be passed freely to which ever method needs it.
        public void BeginTransaction(Customer customer)
        {
            bool willProceed = UserInterface.DisplayWelcomeInstructions(_inventory);
            if (willProceed)
            {
                Transaction(customer);
            }
        }
        
        //This is the main transaction logic think of it like "runGame".  This is where the user will be prompted for the desired soda.
        //grab the desired soda from the inventory.
        //get payment from the user.
        //pass payment to the calculate transaction method to finish up the transaction based on the results.
        private void Transaction(Customer customer)
        {
            string selectedCan = UserInterface.SodaSelection(_inventory);
            Can selectedCanFromInventory = GetSodaFromInventory(selectedCan);
            List<Coin> payment = customer.GatherCoinsFromWallet(selectedCanFromInventory);
            CalculateTransaction(payment, selectedCanFromInventory, customer);
        }
        //Gets a soda from the inventory based on the name of the soda.
        private Can GetSodaFromInventory(string nameOfSoda)
        {
          for (var i = 0; i <= _inventory.Count; i++)
            {
                if (nameOfSoda == _inventory[i].Name)
                {
                    return _inventory[i];
                }
            }
            return null;
        }

        //This is the main method for calculating the result of the transaction.
        //It takes in the payment from the customer, the soda object they selected, and the customer who is purchasing the soda.
        //This is the method that will determine the following:
        //If the payment is greater than the price of the soda, and if the sodamachine has enough change to return: Dispense soda, and change to the customer. x
        //If the payment is greater than the cost of the soda, but the machine does not have ample change: Dispense payment back to the customer. x
        //If the payment is exact to the cost of the soda:  Dispense soda. x
        //If the payment does not meet the cost of the soda: dispense payment back to the customer. x
        private void CalculateTransaction(List<Coin> payment, Can chosenSoda, Customer customer)
        {
            double totalPaySum = TotalCoinValue(payment);
            double totalRegisterSum = TotalCoinValue(_register);
            double changeNeeded = DetermineChange(totalPaySum, chosenSoda.Price);

           if (totalPaySum > chosenSoda.Price && totalRegisterSum > changeNeeded)
            {
                DepositCoinsIntoRegister(payment);
                customer.AddCanToBackpack(GetSodaFromInventory(chosenSoda.Name));
                customer.AddCoinsIntoWallet(GatherChange(changeNeeded));
                UserInterface.EndMessage(chosenSoda.Name, changeNeeded);
            }
           else if (totalPaySum > chosenSoda.Price && totalRegisterSum < changeNeeded)
            {
                customer.AddCoinsIntoWallet(payment);
            }
           else if (totalPaySum == chosenSoda.Price)
            {
                DepositCoinsIntoRegister(payment);
                customer.AddCanToBackpack(GetSodaFromInventory(chosenSoda.Name));
                UserInterface.EndMessage(chosenSoda.Name, changeNeeded);
            }
           else if (totalPaySum < chosenSoda.Price)
            {
                customer.AddCoinsIntoWallet(payment);
            }
        }
        //Takes in the value of the amount of change needed.
        //Attempts to gather all the required coins from the sodamachine's register to make change.
        //Returns the list of coins as change to despense.
        //If the change cannot be made, return null.
        private List<Coin> GatherChange(double changeValue)
        {
            List<Coin> ChangeToDispense = new List<Coin>();
            double changeToDispenseSum = TotalCoinValue(ChangeToDispense);
            double changeNeeded = changeValue - changeToDispenseSum;

            while (changeToDispenseSum != changeValue)
            {
                if (changeNeeded >= .25 && RegisterHasCoin("Quarter"))
                {
                    ChangeToDispense.Add(GetCoinFromRegister("Quarter"));
                    changeToDispenseSum = TotalCoinValue(ChangeToDispense);
                }
                else if (changeNeeded >= .10 && RegisterHasCoin("Dime"))
                {
                    ChangeToDispense.Add(GetCoinFromRegister("Dime"));
                    changeToDispenseSum = TotalCoinValue(ChangeToDispense);
                }
                else if (changeNeeded >= .05 && RegisterHasCoin("Nickel"))
                {
                    ChangeToDispense.Add(GetCoinFromRegister("Nickel"));
                    changeToDispenseSum = TotalCoinValue(ChangeToDispense);
                }
                else if (changeNeeded >= .01 && RegisterHasCoin("Penny"))
                {
                    ChangeToDispense.Add(GetCoinFromRegister("Penny"));
                    changeToDispenseSum = TotalCoinValue(ChangeToDispense);
                }
            }           
            return ChangeToDispense;
        }
        //Reusable method to check if the register has a coin of that name.
        //If it does have one, return true.  Else, false.
        private bool RegisterHasCoin(string name)
        {
            for (var i = 0; i <= _register.Count; i++)
            {
                if (name == _register[i].Name)
                {
                    return true;
                }
            }
            return false;
        }
        //Reusable method to return a coin from the register.
        //Returns null if no coin can be found of that name.
        private Coin GetCoinFromRegister(string name)
        {
            for (var i = 0; i <= _register.Count; i++)
            {
                if (name == _register[i].Name)
                {
                    return _register[i];
                }
            }
            return null;
        }
        //Takes in the total payment amount and the price of can to return the change amount.
        private double DetermineChange(double totalPayment, double canPrice)
        {
            return (totalPayment - canPrice);
        }
        //Takes in a list of coins to returnt he total value of the coins as a double.
        private double TotalCoinValue(List<Coin> payment)
        {
            double sumOfPayment = 0;
            for (int i = 0; i < payment.Count; i++)
            {
                sumOfPayment += payment[i].Value;
            }
            return sumOfPayment;
        }
        //Puts a list of coins into the soda machines register.
        private void DepositCoinsIntoRegister(List<Coin> coins)
        {
           for (var i = 0; i < coins.Count; i++)
            {
                _register.Add(coins[i]);
            }
        }
    }
}
