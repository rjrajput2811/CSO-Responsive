/* ------------------------------------------------------------------------------
 *
 *  # D3.js - bivariate area chart
 *
 *  Demo d3.js bivariate area chart setup with .tsv data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3AreaBivariate = function() {


    //
    // Setup module components
    //

    // Chart
    var _areaBivariate = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-area-bivariate'),
            height = 400;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = {top: 5, right: 10, bottom: 20, left: 40},
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom - 5,
                color = '#ffb980';

            // Format data
            var parseDate = d3.time.format("%Y%m%d").parse;



            // Construct scales
            // ------------------------------

            // Horizontal
            var x = d3.time.scale()
                .range([0, width]);

            // Vertical
            var y = d3.scale.linear()
                .range([height, 0]);



            // Create axes
            // ------------------------------

            // Horizontal
            var xAxis = d3.svg.axis()
                .scale(x)
                .orient("bottom")
                .ticks(6)
                .tickFormat(d3.time.format("%b"));

            // Vertical
            var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left");



            // Create chart
            // ------------------------------

            // Add SVG element
            var container = d3.select(element).append("svg");

            // Add SVG group
            var svg = container
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");



            // Construct chart layout
            // ------------------------------

            // Area
            var area = d3.svg.area()
                .x(function(d) { return x(d.date); })
                .y0(function(d) { return y(d.low); })
                .y1(function(d) { return y(d.high); });



            // Load data
            // ------------------------------

            d3.tsv("../../../../demo_data/d3/lines/lines_bivariate.tsv", function(error, data) {

                // Pull out values
                data.forEach(function(d) {
                    d.date = parseDate(d.date);
                    d.low = +d.low;
                    d.high = +d.high;
                });


                // Set input domains
                // ------------------------------

                // Horizontal
                x.domain(d3.extent(data, function(d) { return d.date; }));

                // Vertical
                y.domain([d3.min(data, function(d) { return d.low; }), d3.max(data, function(d) { return d.high; })]);



                //
                // Append chart elements
                //

                // Add area
                svg.append("path")
                    .datum(data)
                    .attr("class", "d3-area")
                    .attr("fill", color)
                    .attr("d", area);


                // Append axes
                // ------------------------------

                // Horizontal
                svg.append("g")
                    .attr("class", "d3-axis d3-axis-horizontal")
                    .attr("transform", "translate(0," + height + ")")
                    .call(xAxis);

                // Vertical
                var verticalAxis = svg.append("g")
                    .attr("class", "d3-axis d3-axis-vertical")
                    .call(yAxis);

                // Add text label
                verticalAxis.append("text")
                    .attr("class", "d3-axis-title")
                    .attr("transform", "rotate(-90)")
                    .attr("y", 10)
                    .attr("dy", ".71em")
                    .style("text-anchor", "end")
                    .text("Temperature (ºF)");
            });



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

                // Main svg width
                container.attr("width", width + margin.left + margin.right);

                // Width of appended group
                svg.attr("width", width + margin.left + margin.right);


                // Axes
                // -------------------------

                // Horizontal range
                x.range([0, width]);

                // Horizontal axis
                svg.selectAll('.d3-axis-horizontal').call(xAxis);


                // Chart elements
                // -------------------------

                // Area path
                svg.selectAll('.d3-area').attr("d", area);
            }
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _areaBivariate();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3AreaBivariate.init();
});
