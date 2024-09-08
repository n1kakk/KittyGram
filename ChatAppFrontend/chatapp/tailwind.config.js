/** @type {import('tailwindcss').Config} */
module.exports = {
  content: ["./src/**/*.{js,jsx,ts,tsx}"],
  theme: {
    extend: {},
  },
  plugins: [
    require('daisyui'),
  ],
  daisyui: {
    // themes: ['light', 'dark'],
    themes: [
      {
        light: {
          ...require("daisyui/src/theming/themes")["light"],
          ".btn-search": {
            // "border-radius": "66px",
            // "background-color": "#f8eaf5",
            // "border-color": "#f7ddf3",
            "color": "#c2c0c1",
          },
          ".btn-search:hover": {
            // "border-radius": "66px",
            // "background-color": "#fce1f6",
            // "border-color": "#f7ddf3",
            "color": "#a19da0",
          },
          ".conv-pointer:hover": {
            "background-color": "#f8eaf5",
          },

        },
        dark: {
          ...require("daisyui/src/theming/themes")["dark"],
          ".btn-search": {
            // "border-radius": "66px",
            // "background-color": "#402039",
            // "border-color": "#36192f",
            "color": "#c2c0c1",
          },
          ".btn-search:hover": {
            // "border-radius": "66px",
            // "background-color": "#663a5c",
            // "border-color": "#36192f",
            "color": "#a19da0",
          },
          ".conv-pointer:hover": {
            "background-color": "#402039",
          },

        },
      },
    ],
  },
}

