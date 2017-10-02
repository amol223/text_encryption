using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EncryptDecrypt;

namespace Text
{
    class Program
    {
        static void Main(string[] args)
        {
            //from string literal and string concatenation
            string fname, lname;
            fname = "Rowan";
            lname = "Atkinson";

            string fullname = fname + lname;
            Console.WriteLine("Full Name: {0}", fullname);

            //by using string constructor
            char[] letters = { 'H', 'e', 'l', 'l', 'o' };
            string greetings = new string(letters);
            Console.WriteLine("Greetings: {0}", greetings);

            //methods returning string
            string[] sarray = { "Hello", "From", "Tutorials", "Point" };
            string message = String.Join(" ", sarray);
            Console.WriteLine("Message: {0}", message);

            //formatting method to convert a value
            DateTime waiting = new DateTime(2012, 10, 10, 17, 58, 1);
            string chat = String.Format("Message sent at {0:t} on {0:D}", waiting);
            Console.WriteLine("Message: {0}", chat);//from string literal and string concatenation
            
            Console.WriteLine("Enter input string: ");
            string input = Console.ReadLine(); //taking string input from user

            EncryptDecryptClass encdec = new EncryptDecryptClass(); //encrypt decrypt class object

            string encryptedString = encdec.EncryptInput(input); //encrypted string
            Console.WriteLine("Encrypted String: {0}", encryptedString);

            string decryptedString = encdec.DecryptInput(encryptedString); //decrypted string
            Console.WriteLine("Decrypted String: {0}", decryptedString);

            string textenograph = encdec.stringtenograph(encryptedString);
            Console.WriteLine("Textenographed String: {0}", textenograph);

            string decodetextenograph = encdec.decodestringtenograph(textenograph);
            Console.WriteLine("Decoded Textenographed String: {0}", decodetextenograph);

            string stringFromDecodeTextenograph = encdec.DecryptInput(decodetextenograph); //decrypted string
            Console.WriteLine("Decrypted Textenographed String: {0}", stringFromDecodeTextenograph);

            Console.Read();
        }
    }
}
