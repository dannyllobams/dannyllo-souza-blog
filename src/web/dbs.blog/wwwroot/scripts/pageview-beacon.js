(function () {

    const el = document.querySelector('article[data-pageview="post"][data-post-id]');
    if (!el) return;

    const postId = el.dataset.postId;
    if (!postId) return;

    const STORAGE_KEY = `pv:${postId}`;
    const TTL_MINUTES = 30;

    try {
        const last = localStorage.getItem(STORAGE_KEY);
        const now = Date.now();

        if (last && now - Number(last) < TTL_MINUTES * 60 * 1000) {
            return;
        }

        localStorage.setItem(STORAGE_KEY, now.toString());
    } catch {
        // storage indisponível → segue sem dedupe no client
    }

    const payload = JSON.stringify({
        PageId: postId,
        timestamp: Date.now()
    });
    const blob = new Blob([payload], { type: "application/json" });

    if (navigator.sendBeacon) {
        navigator.sendBeacon("/api/pageviews", blob);
    } else {
        fetch("/api/pageviews", {
            method: "POST",
            headers: { "Content-Type": "application/json" },
            body: payload,
            keepalive: true
        });
    }
})();