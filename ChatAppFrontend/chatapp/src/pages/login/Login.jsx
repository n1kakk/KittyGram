﻿import React from 'react'

const Login = () => {
  return (
    <div className='flex flex-col items-center justify-center min-w-96 mx-auto'>
        <div className='w-full p-6 rounded-lg shadow-md bg-gray-400 bg-clip-padding backdrop-filter 
        backdrop-blur-lg bg-opacity-0'>
            <h1 className='text-3x1 font-semibold text-center text-gray-300'>
                Login
                <span className='text-blue-500'> KittyGram</span>
            </h1>

            <form>
              <div>
                <label className='label p-2'>
                    <span className='text-base label-text'>Nickname</span>
                </label>
                <input type='text' placeholder='Enter nickname' className='w-full input input-bordered h-10'/>
              </div> 
              <div>
              <label className='label p-2'>
                    <span className='text-base label-text'>Password</span>
                </label>
                <input type='password' placeholder='Enter password' className='w-full input input-bordered h-10'/>
              </div> 

              <div>
                <button className='btn btn-block btn-sm mt-2'>Login</button>
              </div>
              <a href='#' className='text-sm hover:underline hover:text-blue-600 mt-2 inline-block'>
                {"Don't"} have an account?
              </a>
            </form>
        </div>
    </div>
  )
}

export default Login;

//STARTER CODE FOR THIS FILE
// import React from 'react'

// const Login = () => {
//   return (
//     <div className='flex flex-col items-center justify-center min-w-96 mx-auto'>
//         <div className='w-full p-6 rounded-lg shadow-md bg-gray-400 bg-clip-padding backdrop-filter 
//         backdrop-blur-lg bg-opacity-0'>
//             <h1 className='text-3x1 font-semibold text-center text-gray-300'>
//                 Login
//                 <span className='text-blue-500'> KittyGram</span>
//             </h1>

//             <form>
//               <div>
//                 <label className='label p-2'>
//                     <span className='text-base label-text'>Nickname</span>
//                 </label>
//                 <input type='text' placeholder='Enter nickname' className='w-full input input-bordered h-10'/>
//               </div> 
//               <div>
//               <label className='label p-2'>
//                     <span className='text-base label-text'>Password</span>
//                 </label>
//                 <input type='password' placeholder='Enter password' className='w-full input input-bordered h-10'/>
//               </div> 

//               <div>
//                 <button className='btn btn-block btn-sm mt-2'>Login</button>
//               </div>
//               <a href='#' className='text-sm hover:underline hover:text-blue-600 mt-2 inline-block'>
//                 {"Don't"} have an account?
//               </a>
//             </form>
//         </div>
//     </div>
//   )
// }

// export default Login