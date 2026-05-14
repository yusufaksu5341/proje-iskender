/* =========================================================
   FLAME BID WIDGET
   ========================================================= */
class FlameBidWidget {
    constructor(el) {
        this.el   = el;
        this.step = parseInt(el.dataset.step)  || 50;
        this.min  = parseInt(el.dataset.min)   || 0;
        this.max  = parseInt(el.dataset.max)   || 99999950;
        this.val  = parseInt(el.dataset.value) || this.min;

        this.numEl     = el.querySelector('.flame-bid-num');
        this.particles = el.querySelector('.flame-particles');
        this.plusBtn   = el.querySelector('.flame-btn-plus');
        this.minusBtn  = el.querySelector('.flame-btn-minus');

        this._holdTimer    = null;
        this._holdInterval = null;
        this._plusCount    = 0;

        this._injectBgFlames();
        this._render();
        this._bind();
    }

    _injectBgFlames() {
        const wrap = document.createElement('div');
        wrap.className = 'bid-bg-flames';
        wrap.innerHTML =
            '<div class="aura-ring aura-ring-1"></div>' +
            '<div class="aura-ring aura-ring-2"></div>' +
            '<div class="aura-ring aura-ring-3"></div>' +
            '<div class="bg-glow"></div>';
        this._bgFlames = wrap;
        const display = this.el.querySelector('.flame-bid-display');
        display.style.overflow = 'visible';
        display.insertBefore(wrap, display.firstChild);
    }

    _updateBgFlames() {
        if (this._plusCount >= 2) {
            this._bgFlames.classList.add('active');
            // 2. basışta level 1, her 3 basışta bir kademe yükselir (max 4)
            const level = Math.min(4, Math.floor((this._plusCount - 2) / 3) + 1);
            this._bgFlames.dataset.level = level;
        } else {
            this._bgFlames.classList.remove('active');
            delete this._bgFlames.dataset.level;
        }
    }

    _render() {
        this.numEl.textContent = this.val.toLocaleString('tr-TR');
    }

    _change(delta) {
        const next = Math.min(this.max, Math.max(this.min, this.val + delta));
        if (next === this.val) return;
        this.val = next;

        if (delta > 0) {
            this._plusCount++;
        } else {
            this._plusCount = Math.max(0, this._plusCount - 1);
        }
        this._updateBgFlames();
        this._render();
        this._burst(delta > 0 ? 'hot' : 'cold');
        this._pulse(delta > 0 ? 'up' : 'down');
    }

    _burst(type) {
        const hotColors  = ['#ff2d00','#ff6b00','#ffb700','#ffd700','#ff4500','#ff8800'];
        const coldColors = ['#0055ff','#00aaff','#55ddff','#33bbee','#aaeeff','#0033cc'];
        const colors = type === 'hot' ? hotColors : coldColors;
        const count  = 9 + Math.floor(Math.random() * 6);

        for (let i = 0; i < count; i++) {
            setTimeout(() => {
                const p = document.createElement('div');
                p.className = `fp fp-${type}`;
                const w   = 6  + Math.random() * 14;
                const h   = w  * (1.3 + Math.random() * .7);
                const tx  = (Math.random() - .5) * 110;
                const ty  = -(55 + Math.random() * 65);
                const rot = (Math.random() - .5) * 90;
                const dur = .42 + Math.random() * .46;
                const lft = 8  + Math.random() * 84;
                const top = 15 + Math.random() * 65;
                const clr = colors[Math.floor(Math.random() * colors.length)];
                p.style.cssText = [
                    `width:${w}px`, `height:${h}px`,
                    `left:${lft}%`, `top:${top}%`,
                    `background:radial-gradient(ellipse at 50% 80%, ${clr} 0%, transparent 75%)`,
                    `box-shadow:0 0 ${Math.round(w * .7)}px ${clr}`,
                    `--tx:${tx}px`, `--ty:${ty}px`, `--rot:${rot}deg`,
                    `animation-duration:${dur}s`,
                ].join(';');
                this.particles.appendChild(p);
                p.addEventListener('animationend', () => p.remove(), { once: true });
            }, i * 22);
        }
    }

    _pulse(dir) {
        const cls = dir === 'up' ? 'pulse-up' : 'pulse-down';
        this.numEl.classList.remove('pulse-up', 'pulse-down');
        void this.numEl.offsetWidth;
        this.numEl.classList.add(cls);
        this.numEl.addEventListener('animationend', () => this.numEl.classList.remove(cls), { once: true });
    }

    _startHold(delta) {
        this._change(delta);
        this._holdTimer = setTimeout(() => {
            this._holdInterval = setInterval(() => this._change(delta), 95);
        }, 340);
    }

    _stopHold() {
        clearTimeout(this._holdTimer);
        clearInterval(this._holdInterval);
    }

    _bind() {
        const attach = (btn, delta) => {
            btn.addEventListener('mousedown', () => this._startHold(delta));
            btn.addEventListener('touchstart', e => {
                e.preventDefault();
                this._startHold(delta);
            }, { passive: false });
            ['mouseup', 'mouseleave', 'touchend', 'touchcancel'].forEach(ev =>
                btn.addEventListener(ev, () => this._stopHold())
            );
        };
        attach(this.plusBtn,  this.step);
        attach(this.minusBtn, -this.step);
    }

    getValue() { return this.val; }
}

/* =========================================================
   DOM HAZIR
   ========================================================= */
document.addEventListener('DOMContentLoaded', () => {

    /* Flame bid widget — tüm örnekleri başlat */
    document.querySelectorAll('.flame-bid-wrap[data-step]').forEach(el => {
        el._flameBid = new FlameBidWidget(el);
    });

    /* Geri sayım zamanlayıcıları */
    function formatTime(secs) {
        if (secs <= 0) return 'Sona erdi';
        const years  = Math.floor(secs / 31536000);
        const months = Math.floor(secs / 2592000);
        const weeks  = Math.floor(secs / 604800);
        const days   = Math.floor(secs / 86400);
        const hours  = Math.floor(secs / 3600);
        const mins   = Math.floor((secs % 3600) / 60);
        const s      = secs % 60;
        if (years  >= 1) return years  + ' yıl '   + Math.floor((secs % 31536000) / 2592000) + ' ay';
        if (months >= 1) return months + ' ay '    + Math.floor((secs % 2592000)  / 86400)   + ' gün';
        if (weeks  >= 1) return weeks  + ' hafta ' + Math.floor((secs % 604800)   / 86400)   + ' gün';
        if (days   >= 1) return days   + ' gün '   + (hours % 24) + ' sa';
        return String(hours).padStart(2,'0') + ':' + String(mins).padStart(2,'0') + ':' + String(s).padStart(2,'0');
    }

    document.querySelectorAll('.countdown-timer[data-ends]').forEach(el => {
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
                    if (btn) { btn.textContent = 'Artırma Bitti'; btn.disabled = true; btn.style.opacity = '.5'; }
                }
                return;
            }
            el.textContent = formatTime(remaining);
            if (remaining < 300) el.style.color = 'var(--fire-red)';
        }, 1000);
    });



    /* Kullanıcı dropdown menüsü — tıklayınca aç/kapat */
    const userPill     = document.querySelector('.nav-user-pill');
    const userDropdown = document.querySelector('.nav-user-dropdown');
    if (userPill && userDropdown) {
        userPill.addEventListener('click', e => {
            // Logout linkine tıklanırsa navigasyona izin ver
            if (e.target.closest('.nav-user-logout, .nav-dropdown-logout, .nav-user-dropdown a')) return;
            userDropdown.classList.toggle('open');
            e.stopPropagation();
        });
        // Dropdown içindeki linklere tıklanınca kapat
        userDropdown.addEventListener('click', () => {
            userDropdown.classList.remove('open');
        });
        // Dışarıya tıklanınca kapat
        document.addEventListener('click', e => {
            if (!userPill.contains(e.target)) {
                userDropdown.classList.remove('open');
            }
        });
    }

    /* Hamburger menü */
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

    /* Arama */
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
