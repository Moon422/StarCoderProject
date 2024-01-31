/** @type {import('tailwindcss').Config} */
export default {
  content: [
    "./index.html",
    "./src/**/*.{js,ts,jsx,tsx}",
  ],
  theme: {
    extend: {
      colors: {
        'bs-color': '#00a0da',
        '292f7b': '#292f7b',
        '4975bc': '#4975bc',
      }
    },
  },
  plugins: [],
}
