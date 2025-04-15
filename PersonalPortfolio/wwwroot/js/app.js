// PersonalPortfolio/wwwroot/js/app.js

window.initializeTheme = function() {
    const theme = localStorage.getItem('theme');
    if (theme === 'dark' || (!theme && window.matchMedia('(prefers-color-scheme: dark)').matches)) {
        document.documentElement.classList.add('dark');
    } else {
        document.documentElement.classList.remove('dark');
    }
};

window.initializeAnimation = function() {
    // Add intersection observer for scroll animations
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                entry.target.classList.add('animate-in');
                observer.unobserve(entry.target);
            }
        });
    }, { threshold: 0.1 });

    // Observe all project cards, tech icons, and other animated elements
    document.querySelectorAll('.project-card, .tech-icon').forEach(el => {
        observer.observe(el);
    });
};

window.scrollToElement = function(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        // Add smooth scrolling
        element.scrollIntoView({ 
            behavior: 'smooth',
            block: 'start'
        });
    }
};

// TypeWriter effect for text elements
window.initTypewriter = function(elementId, text, speed = 50) {
    const element = document.getElementById(elementId);
    if (!element) return;
    
    element.textContent = '';
    let i = 0;
    
    function typeWriter() {
        if (i < text.length) {
            element.textContent += text.charAt(i);
            i++;
            setTimeout(typeWriter, speed);
        }
    }
    
    typeWriter();
};

// Parallax effect for background elements
window.initParallax = function() {
    window.addEventListener('scroll', function() {
        const scrollPosition = window.pageYOffset;
        
        document.querySelectorAll('.parallax').forEach(element => {
            const speed = element.getAttribute('data-speed') || 0.5;
            element.style.transform = `translateY(${scrollPosition * speed}px)`;
        });
    });
};

// Initialize all animations when document is ready
document.addEventListener('DOMContentLoaded', function() {
    if (window.initializeTheme) {
        window.initializeTheme();
    }
    
    if (window.initializeAnimation) {
        window.initializeAnimation();
    }
    
    if (window.initParallax) {
        window.initParallax();
    }
});

// Syntax highlighting function
window.highlightCode = function() {
    if (typeof hljs !== 'undefined') {
        document.querySelectorAll('pre code').forEach((el) => {
            hljs.highlightElement(el);
        });
    }
};

// Copy to clipboard function
window.copyToClipboard = function(text) {
    return navigator.clipboard.writeText(text);
};