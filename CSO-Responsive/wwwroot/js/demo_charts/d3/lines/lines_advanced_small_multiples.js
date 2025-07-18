/* ------------------------------------------------------------------------------
 *
 *  # D3.js - small multiples chart
 *
 *  Demo d3.js small multiples chart setup with .csv data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3AreaMultiples = function() {


    //
    // Setup module components
    //

    // Chart
    var _areaMultiples = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-small-multiples'),
            height = 100;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = {top: 5, right: 10, bottom: 5, left: 10},
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom - 5;

            // Format data
            var parseDate = d3.time.format("%b %Y").parse;

            // Colors
            var area_color = '#66bb6a',
                text_color = '#fff';



            // Construct scales
            // ------------------------------

            // Horizontal
            var x = d3.time.scale()
                .range([0, width]);

            // Vertical
            var y = d3.scale.linear()
                .range([height, 0]);



            // Construct chart layout
            // ------------------------------

            // Area
            var area = d3.svg.area()
                .interpolate("monotone")
                .x(function(d) { return x(d.date); })
                .y0(height)
                .y1(function(d) { return y(d.price); });

            // Line
            var line = d3.svg.line()
                .x(function(d) { return x(d.date); })
                .y(function(d) { return y(d.price); });


            // Load data
            // ------------------------------

            d3.csv("../../../../demo_data/d3/lines/lines_small_multiples.csv", function(error, data) {

                // Pull out values
                data.forEach(function(d) {
                    d.price = +d.price;
                    d.date = parseDate(d.date);
                })

                // Nest data by symbol
                var symbols = d3.nest()
                    .key(function(d) { return d.symbol; })
                    .entries(data);

                // Compute the maximum price per symbol, needed for the y-domain.
                symbols.forEach(function(s) {
                    s.maxPrice = d3.max(s.values, function(d) { return d.price; });
                });

                // Compute the minimum and maximum date across symbols.
                // We assume values are sorted by date.
                x.domain([
                    d3.min(symbols, function(s) { return s.values[0].date; }),
                    d3.max(symbols, function(s) { return s.values[s.values.length - 1].date; })
                ]);


                // Create chart
                // ------------------------------

                // Add SVG elements
                var svg = d3Container.selectAll("svg")
                    .data(symbols)
                    .enter()
                    .append("svg")
                        .attr("class", "d3-multiples")
                        .attr("width", width + margin.left + margin.right)
                        .attr("height", height + margin.top + margin.bottom)
                        .append("g")
                            .attr("transform", "translate(" + margin.left + "," + margin.top + ")");


                //
                // Append chart elements
                //

                // Add area
                svg.append("path")
                    .attr("d", function(d) { y.domain([0, d.maxPrice]); return area(d.values); })
                    .attr("class", "d3-area")
                    .style("fill", area_color);

                // Add name label
                svg.append("text")
                    .attr("class", "d3-multiples-label")
                    .attr("x", width - 8)
                    .attr("y", height - 8)
                    .style("fill", text_color)
                    .style("text-anchor", "end")
                    .style("text-weight", 500)
                    .text(function(d) { return d.key; });



                // Resize chart
                // ------------------------------

                // Call function on window resize
                window.addEventListener('resize', resize);

                // Call function on sidebar width change
                var sidebarToggle = document.querySelector('.sidebar-control');
                sidebarToggle && sidebarToggle.addEventListener('click', resize);

                // Resize function
                // 
                // Since D3 doesn't support SVG resize by default,
                // we need to manually specify parts of the graph that need to 
                // be updated on window resize
                function resize() {

                    // Layout variables
                    width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right;


                    // Layout
                    // -------------------------

                    // Resize all multiples
                    d3.selectAll(".d3-multiples").attr("width", width + margin.left + margin.right);

                    // Horizontal range
                    x.range([0, width]);


                    // Chart elements
                    // -------------------------

                    // Area path
                    svg.selectAll('.d3-area').attr("d", function(d) { y.domain([0, d.maxPrice]); return area(d.values); });

                    // Text label
                    svg.selectAll('.d3-multiples-label').attr("x", width - 8);
                }
            });
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _areaMultiples();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3AreaMultiples.init();
});
