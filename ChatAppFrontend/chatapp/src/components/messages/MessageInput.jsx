import React from 'react'
import { PiPawPrintLight } from "react-icons/pi";

const MessageInput = () => {
  return (
    <form className='px-4 my-3'>
        <div className='w-full relative'>
            {/* <input type='text'
                className='border text-sm rounded-lg block w-full p-2.5 bg-[#f8eaf5] border-[#f7ddf3] text-[#eba0e1]'
                placeholder='Message'
            /> */}
            <input type='text'
                className='border text-sm rounded-lg block w-full p-2.5 bg-[#f8eaf5] border-[#f7ddf3] text-[#eba0e1]'
                placeholder='Message'
            />
            <button type='submit' className='absolute inset-y-0 end-0 flex items-center pe-3'>
                <PiPawPrintLight />
            </button>
        </div>
    </form>
  )
}

export default MessageInput;