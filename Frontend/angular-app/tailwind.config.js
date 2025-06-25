/** @type {import('tailwindcss').Config} */
module.exports = {
  content: [
    "./src/**/*.{html,ts}", // ✅ Important for Angular
  ],
  theme: {
    extend: {
      keyframes: {
        fadeIn: {
          from: { opacity: '0' },
          to: { opacity: '0.5' },
        },
        fadeOut: {
          '0%': { opacity: '0.5' },
          '100%': { opacity: '0' },
        },
        slideMenu: {
          '0%': { transform: 'translateX(-100%)' },
          '100%': { transform: 'translateX(0)' },
        },
        slideMenuReverse: {
          '0%': { transform: 'translateX(0)' },
          '100%': { transform: 'translateX(-100%)' },
        },
      },
      animation: {
        fadeBackdrop: 'fadeIn 0.3s ease forwards',
        fadeBackdropReverse: 'fadeOut 300ms ease-out forwards' ,

        slideMenu: 'slideMenu 0.3s ease forwards',
        slideMenuReverse: 'slideMenuReverse 0.3s ease forwards',
      },
    },
  },
  plugins: [],
}
