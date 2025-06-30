/** @type {import('tailwindcss').Config} */
module.exports = {
    content: [
        "./src/**/*.{html,ts}",
    ],
    theme: {
        extend: {

            // colors
            colors: {
                primary: '#365ae3',
                secondary: '#fb923c',
                accent: '#facc15',
                neutral: '#3d4451',
                'base-100': '#ffffff',
                'base-200': '#f4f4f4',
                'base-300': '#e4e4e7',
                success: '#2dd36f',
                error: '#f93e3e',
                warning: '#ff9f00',
                info: '#0c9eff',
            },

            // keyframes
            keyframes: {
                fadeIn: {
                    from: {opacity: '0'},
                    to: {opacity: '0.5'},
                },
                fadeOut: {
                    '0%': {opacity: '0.5'},
                    '100%': {opacity: '0'},
                },
                slideMenu: {
                    '0%': {transform: 'translateX(-100%)'},
                    '100%': {transform: 'translateX(0)'},
                },
                slideMenuReverse: {
                    '0%': {transform: 'translateX(0)'},
                    '100%': {transform: 'translateX(-100%)'},
                },
            },

            // animations
            animation: {
                fadeBackdrop: 'fadeIn 0.3s ease forwards',
                fadeBackdropReverse: 'fadeOut 300ms ease-out forwards',

                slideMenu: 'slideMenu 0.3s ease forwards',
                slideMenuReverse: 'slideMenuReverse 0.3s ease forwards',
            },
        },
    },
    plugins: [],
}
