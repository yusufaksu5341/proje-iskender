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

        this._render();
        this._bind();
    }

    _render() {
        this.numEl.textContent = this.val.toLocaleString('tr-TR');
    }

    _change(delta) {
        const next = Math.min(this.max, Math.max(this.min, this.val + delta));
        if (next === this.val) return;
        this.val = next;
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
                const p   = document.createElement('div');
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
                    `width:${w}px`,
                    `height:${h}px`,
                    `left:${lft}%`,
                    `top:${top}%`,
                    `background:radial-gradient(ellipse at 50% 80%, ${clr} 0%, transparent 75%)`,
                    `box-shadow:0 0 ${Math.round(w * .7)}px ${clr}`,
                    `--tx:${tx}px`,
                    `--ty:${ty}px`,
                    `--rot:${rot}deg`,
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

document.addEventListener('DOMContentLoaded', () => {
    document.querySelectorAll('.flame-bid-wrap[data-step]').forEach(el => {
        el._flameBid = new FlameBidWidget(el);
    });
});

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
