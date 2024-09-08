import React from 'react'
import Sidebar from '../../components/messages/sidebar/Sidebar';
import MessageContainer from '../../components/messages/MessageContainer';

const Home = () => {
  return (
    <div className='flex flex-1 justify-center items-center'>

    <div className='relative flex sm:h-[650px] md:h-[550px] w-full md:w-3/4 rounded-lg overflow-hidden bg-gray-400 bg-clip-padding backdrop-filter backdrop-blur-lg bg-opacity-0'>
   <div className='absolute inset-0 border-2 border-gray-500 rounded-lg pointer-events-none '></div>

        <Sidebar/>
        <MessageContainer />
    </div>
    </div>

  )
}

export default Home;