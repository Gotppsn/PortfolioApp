// PersonalPortfolio/wwwroot/js/app.js

// Theme handling
window.initializeTheme = function() {
    const theme = localStorage.getItem('theme');
    const prefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
    
    if (theme === 'dark' || (!theme && prefersDark)) {
        document.documentElement.classList.add('dark');
    } else {
        document.documentElement.classList.remove('dark');
    }
    
    // Listen for OS theme changes
    window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
        const newTheme = e.matches ? 'dark' : 'light';
        if (!localStorage.getItem('theme')) {
            // Only apply OS preference if user hasn't manually chosen a theme
            document.documentElement.classList.toggle('dark', e.matches);
        }
    });
};

// Animation handling
window.initializeAnimation = function() {
    // Add intersection observer for scroll animations
    const observer = new IntersectionObserver((entries) => {
        entries.forEach(entry => {
            if (entry.isIntersecting) {
                // Stagger animations by adding delay
                const index = Array.from(entry.target.parentElement.children).indexOf(entry.target);
                const delay = index * 100;
                entry.target.style.animationDelay = `${delay}ms`;
                entry.target.classList.add('animate-in');
                observer.unobserve(entry.target); // Stop observing once animated
            }
        });
    }, { 
        threshold: 0.1,
        rootMargin: "0px 0px -50px 0px" // Trigger slightly before element is in view
    });

    // Observe all animated elements
    document.querySelectorAll('.project-card, .tech-icon, .animate-on-scroll, .skill-card').forEach(el => {
        observer.observe(el);
    });
};

// Parallax effect for hero section
window.initializeParallax = function() {
    if (window.innerWidth >= 768) { // Only on desktop
        const parallaxElements = document.querySelectorAll('.parallax-element');
        
        window.addEventListener('mousemove', function(e) {
            const mouseX = e.clientX / window.innerWidth - 0.5;
            const mouseY = e.clientY / window.innerHeight - 0.5;

            parallaxElements.forEach(el => {
                const speed = el.dataset.speed || 25;
                const x = mouseX * speed;
                const y = mouseY * speed;
                el.style.transform = `translate(${x}px, ${y}px)`;
            });
        });
    }
};

// Smooth scrolling to element
window.scrollToElement = function(elementId) {
    const element = document.getElementById(elementId);
    if (element) {
        element.scrollIntoView({ 
            behavior: 'smooth',
            block: 'start'
        });
    }
};

// Typewriter effect
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

// Code syntax highlighting
window.highlightCode = function() {
    if (typeof hljs !== 'undefined') {
        document.querySelectorAll('pre code').forEach((el) => {
            hljs.highlightElement(el);
        });
    }
};

// Copy to clipboard with feedback
window.copyToClipboard = function(text, feedbackId) {
    navigator.clipboard.writeText(text).then(() => {
        const feedback = document.getElementById(feedbackId);
        if (feedback) {
            feedback.classList.remove('opacity-0');
            feedback.classList.add('opacity-100');
            
            setTimeout(() => {
                feedback.classList.remove('opacity-100');
                feedback.classList.add('opacity-0');
            }, 2000);
        }
    });
};

// Back to top button
window.initializeBackToTop = function() {
    const backToTopButton = document.getElementById('back-to-top');
    if (!backToTopButton) return;
    
    window.addEventListener('scroll', function() {
        if (window.scrollY > 300) {
            backToTopButton.classList.remove('opacity-0', 'pointer-events-none');
            backToTopButton.classList.add('opacity-100', 'pointer-events-auto');
        } else {
            backToTopButton.classList.remove('opacity-100', 'pointer-events-auto');
            backToTopButton.classList.add('opacity-0', 'pointer-events-none');
        }
    });
    
    backToTopButton.addEventListener('click', function() {
        window.scrollTo({
            top: 0,
            behavior: 'smooth'
        });
    });
};

// Add loading lazy for images
window.lazyLoadImages = function() {
    if ('loading' in HTMLImageElement.prototype) {
        const images = document.querySelectorAll('img[loading="lazy"]');
        images.forEach(img => {
            img.src = img.dataset.src;
        });
    } else {
        // Fallback for browsers that don't support lazy loading
        const script = document.createElement('script');
        script.src = 'https://cdnjs.cloudflare.com/ajax/libs/lazysizes/5.3.2/lazysizes.min.js';
        document.body.appendChild(script);
    }
};

// Initialize all when document is ready
document.addEventListener('DOMContentLoaded', function() {
    window.initializeTheme();
    window.initializeAnimation();
    window.initializeParallax();
    window.highlightCode();
    window.initializeBackToTop();
    window.lazyLoadImages();
});