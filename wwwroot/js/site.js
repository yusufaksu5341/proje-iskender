document.addEventListener('DOMContentLoaded', () => {

    const timers = document.querySelectorAll('.countdown-timer[data-ends]');

    function formatTime(secs) {
        const h = Math.floor(secs / 3600);
        const m = Math.floor((secs % 3600) / 60);
        const s = secs % 60;
        return `${h}s ${String(m).padStart(2, '0')}d ${String(s).padStart(2, '0')}sn`;
    }

    timers.forEach(el => {
        let remaining = parseInt(el.dataset.ends, 10);
        el.textContent = formatTime(remaining);

        const interval = setInterval(() => {
            remaining--;
            if (remaining <= 0) {
                clearInterval(interval);
                el.textContent = 'Sona erdi';
                el.style.color = '#888';
                const card = el.closest('.listing-card');
                if (card) {
                    const btn = card.querySelector('.btn-bid');
                    if (btn) {
                        btn.textContent = 'Artırma Bitti';
                        btn.disabled = true;
                        btn.style.opacity = '.5';
                    }
                }
                return;
            }
            el.textContent = formatTime(remaining);
            if (remaining < 300) {
                el.style.color = 'var(--fire-red)';
            }
        }, 1000);
    });

    document.querySelectorAll('.wishlist-btn').forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.stopPropagation();
            const active = btn.dataset.active === '1';
            btn.dataset.active = active ? '0' : '1';
            btn.textContent = active ? 'Favori' : 'Favoride';
            btn.style.color = active ? '' : 'var(--fire-red)';
            btn.style.borderColor = active ? '' : 'var(--fire-red)';
        });
    });

    const menuToggle = document.getElementById('menuToggle');
    const mobileMenu = document.getElementById('mobileMenu');
    if (menuToggle && mobileMenu) {
        menuToggle.addEventListener('click', () => {
            const open = mobileMenu.classList.toggle('open');
            menuToggle.classList.toggle('open', open);
        });
        document.addEventListener('click', e => {
            if (!menuToggle.contains(e.target) && !mobileMenu.contains(e.target)) {
                mobileMenu.classList.remove('open');
                menuToggle.classList.remove('open');
            }
        });
    }

    const searchInput = document.querySelector('.nav-search input');
    if (searchInput) {
        searchInput.addEventListener('keydown', e => {
            if (e.key === 'Enter') {
                const q = e.target.value.trim();
                if (q) window.location.href = '/ilanlar?q=' + encodeURIComponent(q);
            }
        });
    }

});
