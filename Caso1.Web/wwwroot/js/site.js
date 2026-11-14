// Please see documentation at https://learn.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

document.addEventListener("DOMContentLoaded", () => {
    const themeButton = document.getElementById("theme-toggle");

    if (themeButton) {
        const label = themeButton.querySelector(".theme-toggle-label");
        const updateThemeButton = () => {
            const isDark = document.documentElement.dataset.theme === "dark";
            themeButton.setAttribute("aria-pressed", String(isDark));
            themeButton.setAttribute("aria-label", isDark ? "Activar modo claro" : "Activar modo oscuro");
            label.textContent = isDark ? "Modo claro" : "Modo oscuro";
        };

        updateThemeButton();
        themeButton.addEventListener("click", () => {
            const isDark = document.documentElement.dataset.theme === "dark";
            if (isDark) {
                delete document.documentElement.dataset.theme;
            } else {
                document.documentElement.dataset.theme = "dark";
            }

            try {
                localStorage.setItem("sismo-theme", isDark ? "light" : "dark");
            } catch (_) { }

            updateThemeButton();
        });
    }

    const pagination = document.querySelector(".pagination");
    if (!pagination) return;

    const cards = Array.from(document.querySelectorAll(".quake-card"));
    const pageSize = Number(pagination.dataset.pageSize);
    const pageCount = Math.ceil(cards.length / pageSize);
    const summary = pagination.querySelector(".pagination-summary");
    const numbers = pagination.querySelector(".page-numbers");
    const previous = pagination.querySelector(".page-prev");
    const next = pagination.querySelector(".page-next");
    let currentPage = 1;

    const pageButton = (page) => {
        const button = document.createElement("button");
        button.type = "button";
        button.className = "page-number";
        button.textContent = String(page);
        button.setAttribute("aria-label", `Página ${page}`);
        if (page === currentPage) button.setAttribute("aria-current", "page");
        button.addEventListener("click", () => showPage(page, true));
        return button;
    };

    const showPage = (page, scrollToResults = false) => {
        currentPage = Math.min(Math.max(page, 1), pageCount);
        const first = (currentPage - 1) * pageSize;
        cards.forEach((card, index) => { card.hidden = index < first || index >= first + pageSize; });
        summary.textContent = `Mostrando ${first + 1}–${Math.min(first + pageSize, cards.length)} de ${cards.length} sismos`;
        previous.disabled = currentPage === 1;
        next.disabled = currentPage === pageCount;

        numbers.replaceChildren();
        const pages = [1, currentPage - 1, currentPage, currentPage + 1, pageCount]
            .filter((value, index, all) => value >= 1 && value <= pageCount && all.indexOf(value) === index)
            .sort((a, b) => a - b);

        pages.forEach((value, index) => {
            if (index > 0 && value - pages[index - 1] > 1) {
                const gap = document.createElement("span");
                gap.className = "page-gap";
                gap.textContent = "…";
                gap.setAttribute("aria-hidden", "true");
                numbers.append(gap);
            }
            numbers.append(pageButton(value));
        });

        if (scrollToResults) {
            const firstCard = cards[first];
            firstCard.setAttribute("tabindex", "-1");
            firstCard.focus({ preventScroll: true });
            firstCard.scrollIntoView({
                behavior: window.matchMedia("(prefers-reduced-motion: reduce)").matches ? "auto" : "smooth",
                block: "start"
            });
        }
    };

    previous.addEventListener("click", () => showPage(currentPage - 1, true));
    next.addEventListener("click", () => showPage(currentPage + 1, true));
    showPage(1);
    pagination.hidden = false;
});
