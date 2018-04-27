var searchUrl = searchUrl || "/Search/Submit";
let searchTimerId = null;

const app = new Vue({
    el: "#v_app",
    data: {
        query: "",
        results: [],
        resultsEmpty: false,
        next: null,
        searching: false,
        autoSearch: true
    },
    created: function () {
        this.onHashChange();
    },
    mounted: function() {
        window.addEventListener('hashchange', this.onHashChange, false);
    },
    beforeDestroy: function() {
        window.removeEventListener('hashchange', this.onHashChange, false);
    },
    watch: {
        query: function() {
            // Update hash
            if (this.query) {
                const hash = '#q=' + this.query;
                if (history.pushState) {
                    history.replaceState(null, null, hash);
                } else {
                    location.replace(hash);
                }
            } else {
                if (history.pushState) {
                    history.replaceState(null, null, window.location.pathname)
                } else {
                    location.replace('#');
                }
            }

            // Auto-search
            if (this.autoSearch) {
                if (searchTimerId) {
                    clearTimeout(searchTimerId);
                }
    
                this.results = [];
                this.resultsEmpty = false;
    
                searchTimerId = setTimeout(() => {
                    this.search();
                }, 800);
            }
        }
    },
    methods: {
        onHashChange: function() {
            if (location.hash.substr(0, 3) === '#q=') {
                this.search(location.hash.substr(3));
            }
        },
        getCommaSeperated: function (listItems) {
            if (typeof listItems === "object") {
                listItems = Object.values(listItems);
            }

            if (listItems == null || listItems.length === 0) {
                return "";
            }
            
            var response = "";

            for (let i = 0; i < listItems.length; i++) {
                response += `"${listItems[i]}"`;

                if (i + 2 === listItems.length) {
                    response += " and ";
                } else if (i + 1 < listItems.length) {
                    response += ", ";
                } 
            }

            return response;
        },
        search: function (query) {
            if (query) {
                this.query = query;

                // We disable auto-search because the 'query' is being watched.
                // When the view was rendered, we re-enable auto-search.
                if (this.autoSearch) {
                    this.autoSearch = false;
                    this.$nextTick(function () {
                        this.autoSearch = true;
                    })
                }
            } else {
                query = this.query;
            }

            if (query.length === 0 || this.searching) {
                return;
            }

            this.searching = true;

            if (searchTimerId) {
                clearTimeout(searchTimerId);
                searchTimerId = null;
            }

            this.results = [];
            this.resultsEmpty = false;
            this.next = null;

            fetch(searchUrl + "?q=" + encodeURIComponent(query))
                .then(res => res.json())
                .then(res => {
                    this.searching = false;
                    this.results = res.results;
                    this.resultsEmpty = this.results.length === 0;
                    this.next = res.next;
                });
        }
    }
});