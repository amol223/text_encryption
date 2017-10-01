using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace EncryptDecrypt
{
    public class EncryptDecryptClass //encrypt decrypt class
    {
        
        public EncryptDecryptClass()
        {
            
        }

        public string EncryptInput(string input)
        {
            string key = (ConfigurationManager.AppSettings["key"]); //read encrypt key from config file
            UInt32 sumOfKeyChar = (UInt32)key.Select(x => (int)x).Sum(); //sum of ascii value of all chars in key
            //Console.WriteLine(sumOfKeyChar);
            int lengthOfKey = key.Length; //length of key
            //Console.WriteLine(lengthOfKey);
            UInt32 initialValue = GetInitValue(sumOfKeyChar, lengthOfKey); //compute initial value
            //Console.WriteLine(initialValue);
            string output = startEncryption(input, (int)initialValue); //encrypt function
            //Console.WriteLine(output);
            return output;
        }

        public string DecryptInput(string input)
        {
            string key = (ConfigurationManager.AppSettings["key"]); //read encrypt key from config file
            UInt32 sumOfKeyChar = (UInt32)key.Select(x => (int)x).Sum(); //sum of ascii value of all chars in key
            //Console.WriteLine(sumOfKeyChar);
            int lengthOfKey = key.Length; //length of key
            //Console.WriteLine(lengthOfKey);
            UInt32 initialValue = GetInitValue(sumOfKeyChar, lengthOfKey); //compute initial value
            //Console.WriteLine(initialValue);
            string output = startDecryption(input, (int)initialValue); //decrypt function
            //Console.WriteLine(output);
            return output;
        }

        private UInt32 GetInitValue(UInt32 sumOfKeyChar, int shift)
        {
            string shiftedBitString = rightRotateShift(Convert.ToString(sumOfKeyChar, 2).ToString(), 7); //right circular bit shift
            //Console.WriteLine(shiftedBitString);
            UInt32 initialValue = Convert.ToUInt32(shiftedBitString, 2); //initial shift value
            //Console.WriteLine(initialValue);
            return initialValue;
        }

        private string rightRotateShift(string key, int shift) //right circular bit shift
        {
            shift %= key.Length;
            return key.Substring(key.Length - shift) + key.Substring(0, key.Length - shift);
        }

        private string startEncryption(string input, int initialValue) //encryption function
        {
            StringBuilder inputCharArray = new StringBuilder(input); //string input to array of chars
            List<string> specialCharPositions = new List<string>();  //list of special char positions          
            int offset = 0; //shift value

            //loop each char in input string for encryption
            for(int count = 0; count < inputCharArray.Length; count++)
            {
                offset = initialValue % 10; //shift value for char
                
                if (count % 2 == 0) //even place char
                {
                    if (inputCharArray[count] >= 65 && inputCharArray[count] <= 90) //uppercase alphabet validation
                    {
                        if ((inputCharArray[count] - offset) < 65) //circular shift overflow
                            inputCharArray[count] = Convert.ToChar(91 - (offset - (inputCharArray[count] - 65 )));
                        else
                            inputCharArray[count] = Convert.ToChar((inputCharArray[count] - offset));
                    }
                    else if (inputCharArray[count] >= 97 && inputCharArray[count] <= 122) //lowercase alphabet validation
                    {
                        if ((inputCharArray[count] - offset) < 97) //circular shift overflow
                            inputCharArray[count] = Convert.ToChar(123 - (offset - (inputCharArray[count] - 97)));
                        else
                            inputCharArray[count] = Convert.ToChar((inputCharArray[count] - offset));
                    }
                    else 
                    {
                        //nonalphabet positions to list
                        inputCharArray[count] = Convert.ToChar((inputCharArray[count] + offset));
                        specialCharPositions.Add(count.ToString());
                        specialCharPositions.Add("."); //split specifier
                    }
                }
                else
                {
                    if (inputCharArray[count] >= 65 && inputCharArray[count] <= 90) 
                    {
                        if ((inputCharArray[count] + offset) > 90)
                            inputCharArray[count] = Convert.ToChar(64 + (offset - (90 - inputCharArray[count])));
                        else
                            inputCharArray[count] = Convert.ToChar((inputCharArray[count] + offset));
                    }
                    else if (inputCharArray[count] >= 97 && inputCharArray[count] <= 122)
                    {
                        if ((inputCharArray[count] + offset) > 122)
                            inputCharArray[count] = Convert.ToChar(96 + (offset - (122 - inputCharArray[count])));
                        else
                            inputCharArray[count] = Convert.ToChar((inputCharArray[count] + offset));
                    }
                    else
                    {
                        
                        inputCharArray[count] = Convert.ToChar((inputCharArray[count] + offset));
                        specialCharPositions.Add(count.ToString());
                        specialCharPositions.Add(".");
                    }
                }   
             
                initialValue++;
            }

            if(specialCharPositions.Count !=  0)
                specialCharPositions.Add(specialCharPositions.Count.ToString()); //number of nonalphabetic chars
            else
                specialCharPositions.Add("o"); //no nonalphabetic char case
            string specialCharString = String.Join(String.Empty, specialCharPositions.ToArray()); 
            //Console.WriteLine(specialCharString);
            inputCharArray.Append(".");
            inputCharArray.Append(specialCharString);
            //Console.WriteLine(inputCharArray.ToString());
            string encryptedString = inputCharArray.ToString(); //encrypted string
            //Console.WriteLine(encryptedString);
            return encryptedString;
        }

        private string startDecryption(string input, int initialValue) //decryption function
        {
            string[] splitString = input.Split('.'); //split char and nonalphabets
            string stringToDecrypt = splitString[0]; //actual string to decrypt
            //Console.WriteLine(stringToDecrypt);
            splitString[0] = ""; //eliminate string to decrypt
            splitString[splitString.Length - 1] = ""; //eliminate number of nonalphabets
            //string specialCharPositionString = string.Concat(splitString); //
            //Console.WriteLine(specialCharPositionString);
            
            StringBuilder inputCharArray = new StringBuilder(stringToDecrypt); //char array of string
            int offset = 0; // shift offset

            //loop for each char to decrypt
            for (int count = 0; count < inputCharArray.Length; count++)
            {
                offset = initialValue % 10; //last digit shift value

                if (count % 2 != 0) // odd place char
                {
                    if (!splitString.Contains(count.ToString())) // no nonalphabetic char
                    {
                        if (inputCharArray[count] >= 65 && inputCharArray[count] <= 90) //uppercase alphabet validation
                        {
                            if ((inputCharArray[count] - offset) < 65) //alphabet overflow
                                inputCharArray[count] = Convert.ToChar(91 - (offset - (inputCharArray[count] - 65)));
                            else
                                inputCharArray[count] = Convert.ToChar((inputCharArray[count] - offset));
                        }
                        else if (inputCharArray[count] >= 97 && inputCharArray[count] <= 122) //lowercase alphabet validation
                        {
                            if ((inputCharArray[count] - offset) < 97) //alphabet overflow
                                inputCharArray[count] = Convert.ToChar(123 - (offset - (inputCharArray[count] - 97)));
                            else
                                inputCharArray[count] = Convert.ToChar((inputCharArray[count] - offset));
                        }                        
                    }
                    else
                    {
                        //nonalphabetic char decryption
                        inputCharArray[count] = Convert.ToChar((inputCharArray[count] - offset));
                    }
                }
                else //even place char decryption
                {
                    if (!splitString.Contains(count.ToString()))
                    {
                        if (inputCharArray[count] >= 65 && inputCharArray[count] <= 90)
                        {
                            if ((inputCharArray[count] + offset) > 90)
                                inputCharArray[count] = Convert.ToChar(64 + (offset - (90 - inputCharArray[count])));
                            else
                                inputCharArray[count] = Convert.ToChar((inputCharArray[count] + offset));
                        }
                        else if (inputCharArray[count] >= 97 && inputCharArray[count] <= 122)
                        {
                            if ((inputCharArray[count] + offset) > 122)
                                inputCharArray[count] = Convert.ToChar(96 + (offset - (122 - inputCharArray[count])));
                            else
                                inputCharArray[count] = Convert.ToChar((inputCharArray[count] + offset));
                        }                        
                    }
                    else
                    {
                        inputCharArray[count] = Convert.ToChar((inputCharArray[count] - offset));
                    }
                }

                initialValue++;
            }

            string decryptedString = inputCharArray.ToString(); //decrypted string
            //Console.WriteLine(decryptedString);                        
            return decryptedString;
        }        
    }
}