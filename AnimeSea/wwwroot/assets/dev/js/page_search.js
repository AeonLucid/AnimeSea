var searchUrl = searchUrl || "/Search/Submit";
let searchTimerId = null;

function getParams(query) {
    if (!query) {
        return {};
    }

    return (/^[?#]/.test(query) ? query.slice(1) : query)
        .split('&')
        .reduce((params, param) => {
            let [key, value] = param.split('=');
            params[key] = value ? decodeURIComponent(value.replace(/\+/g, ' ')) : '';
            return params;
        }, {});
}

function getQuery(params) {
    return Object.keys(params)
        .filter(key => params[key] !== null)
        .map(key => key + '=' + encodeURIComponent(params[key]))
        .join('&');
}

const app = new Vue({
    el: "#v_app",
    data: {
        query: "",
        results: [],
        resultsEmpty: false,
        next: null,
        searching: false,
        autoSearch: true,
        providerId: 0
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
                const params = {pid: this.providerId !== 0 ? this.providerId: null, q: this.query}
                const hash = '#' + getQuery(params);

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
        /**
         * See 'page_search.popout.js' for the implementation of this function.
         */
        popOut: popOut,

        /**
         * Add the serie to the library.
         *
         * @param {String} id The ID of the serie.
         */
        add: function(id) {
            // TODO: Implement this.
        },

        /**
         * Called when the hash in the URL was changed.
         */
        onHashChange: function() {
            if (location.hash) {
                const params = getParams(location.hash);
                this.search(params.q, params.pid);
            }
        },
        
        /**
         * Joins all elements of an array or object into a string.
         * 
         * @argument {Array|Object} listItems The array
         * @returns {String} A string with all array elements joined.
         */
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

        /**
         * Searches the given query.
         * 
         * @argument {String} [query] The query. If no argument is given the "this.query" will be used instead.
         */
        search: function (query, providerId) {
            if (query === undefined) {
                query = this.query;
            } else {
                this.query = query;

                // We disable auto-search because the 'query' is being watched.
                // When the view was rendered, we re-enable auto-search.
                if (this.autoSearch) {
                    this.autoSearch = false;
                    this.$nextTick(function() {
                        this.autoSearch = true;
                    });
                }
            }

            if (providerId === undefined) {
                providerId = this.providerId;
            } else {
                this.providerId = providerId;
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

            fetch(searchUrl + "?" + getQuery({q: query, pid: providerId}))
                .then(res => res.json())
                .then(res => {
                    const libaryIds = res.libraryResults.map(e => e.providerSerieId);

                    this.searching = false;
                    this.results = res.results.map(function (result) {
                        // TODO: Add redirect link.
                        result.alreadyAdded = libaryIds.indexOf(result.id) !== -1;

                        return result;
                    });
                    this.resultsEmpty = this.results.length === 0;
                    this.next = res.next;
                });
        }
    }
});