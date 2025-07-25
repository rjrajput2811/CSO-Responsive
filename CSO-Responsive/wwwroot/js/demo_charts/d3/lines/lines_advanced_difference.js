/* ------------------------------------------------------------------------------
 *
 *  # D3.js - difference line chart
 *
 *  Demo d3.js difference line chart setup with .tsv data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3LineDifference = function() {


    //
    // Setup module components
    //

    // Chart
    var _lineDifference = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-difference'),
            height = 400;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var d3Container = d3.select(element),
                margin = {top: 5, right: 10, bottom: 20, left: 40},
                width = d3Container.node().getBoundingClientRect().width - margin.left - margin.right,
                height = height - margin.top - margin.bottom - 5;

            // Format data
            var parseDate = d3.time.format("%Y%m%d").parse;

            // Colors
            var color_above = '#d87a80',
                color_below = '#5ab1ef',
                color_line = '#e5cf0d';



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
                .ticks(6);

            // Vertical
            var yAxis = d3.svg.axis()
                .scale(y)
                .orient("left");



            // Create chart
            // ------------------------------

            // Add SVG element
            var container = d3Container.append("svg");

            // Add SVG group
            var svg = container
                .attr("width", width + margin.left + margin.right)
                .attr("height", height + margin.top + margin.bottom)
                .append("g")
                    .attr("transform", "translate(" + margin.left + "," + margin.top + ")");



            // Construct chart layout
            // ------------------------------

            // Line
            var line = d3.svg.area()
                .interpolate("basis")
                .x(function(d) { return x(d.date); })
                .y(function(d) { return y(d["New York"]); });

            // Area
            var area = d3.svg.area()
                .interpolate("basis")
                .x(function(d) { return x(d.date); })
                .y1(function(d) { return y(d["New York"]); });



            // Load data
            // ------------------------------

            d3.tsv("../../../../demo_data/d3/lines/lines_difference.tsv", function(error, data) {

                // Pull out values
                data.forEach(function(d) {
                    d.date = parseDate(d.date);
                    d["New York"]= +d["New York"];
                    d["San Francisco"] = +d["San Francisco"];
                });

                // Bind data
                svg.datum(data);


                // Set input domains
                // ------------------------------

                // Horizontal
                x.domain(d3.extent(data, function(d) { return d.date; }));

                // Vertical
                y.domain([
                    d3.min(data, function(d) { return Math.min(d["New York"], d["San Francisco"]); }),
                    d3.max(data, function(d) { return Math.max(d["New York"], d["San Francisco"]); })
                ]);


                //
                // Append chart elements
                //

                // Add masks
                // ------------------------------

                svg.append("clipPath")
                  .attr("id", "clip-below")
                  .append("path")
                    .attr("d", area.y0(height));

                svg.append("clipPath")
                  .attr("id", "clip-above")
                  .append("path")
                    .attr("d", area.y0(0));

                svg.append("path")
                  .attr("class", "area mask-above")
                  .attr("clip-path", "url(#clip-above)")
                  .attr("fill", color_above)
                  .attr("d", area.y0(function(d) { return y(d["San Francisco"]); }));

                svg.append("path")
                  .attr("class", "area mask-below")
                  .attr("clip-path", "url(#clip-below)")
                  .attr("fill", color_below)
                  .attr("d", area);


                // Add line
                svg.append("path")
                    .attr("class", "d3-line d3-line-medium")
                    .style("stroke", color_line)
                    .attr("d", line);



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

                // Line path
                svg.selectAll('.d3-line').attr("d", line);


                // Bottom clip
                svg.select('#clip-below path').attr("d", area.y0(height));

                // Top clip
                svg.select('#clip-above path').attr("d", area.y0(0));


                // Top mask
                svg.select('.mask-above').attr("d", area.y0(function(d) { return y(d["San Francisco"]); }))

                // Bottom mask
                svg.select('.mask-below').attr("d", area);

            }
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _lineDifference();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3LineDifference.init();
});
