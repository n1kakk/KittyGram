import React from 'react'
import { IoIosSearch } from "react-icons/io";
import { Input , InputRightElement, InputGroup} from "@chakra-ui/react";

const SearchInput = () => {
  return (

    <div className=" w-full relative">
      
      <InputGroup size='md'>
      <Input type='text' placeholder='Search...' size='md' className='border text-sm rounded-lg block w-full p-2.5
       border-[#d5d2d5] caret-[#f27bd8] text-[#463f46]' focusBorderColor='#f8eaf5'/> 
       <InputRightElement width='4.5rem'>
       <button type='submit' className='absolute inset-y-0 right-0 flex items-center pe-2'>
        <IoIosSearch className='w-5 h-6 outline-none text-gray-400' strokeWidth="0" />
      </button>
       </InputRightElement>
      </InputGroup>
    </div> 



  )
}

export default SearchInput;