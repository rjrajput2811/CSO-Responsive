/* ------------------------------------------------------------------------------
 *
 *  # D3.js - basic sunbirst diagram
 *
 *  Demo sunbirst diagram setup with .json data source
 *
 * ---------------------------------------------------------------------------- */


// Setup module
// ------------------------------

var D3SunburstBasic = function() {


    //
    // Setup module components
    //

    // Chart
    var _sunburstBasic = function() {
        if (typeof d3 == 'undefined') {
            console.warn('Warning - d3.min.js is not loaded.');
            return;
        }

        // Main variables
        var element = document.getElementById('d3-sunburst-basic'),
            width = 400,
            height = 400;


        // Initialize chart only if element exsists in the DOM
        if(element) {

            // Basic setup
            // ------------------------------

            // Define main variables
            var radius = Math.min(width, height) / 2,
                color = d3.scale.category20();



            // Create chart
            // ------------------------------

            var svg = d3.select(element).append("svg")
                .attr("width", width)
                .attr("height", height)
                .append("g")
                    .attr("transform", "translate(" + width / 2 + "," + height / 2 + ")");



            // Construct chart layout
            // ------------------------------

            // Partition layout
            var partition = d3.layout.partition()
                .sort(null)
                .size([2 * Math.PI, radius * radius])
                .value(function(d) { return 1; });

            // Arc
            var arc = d3.svg.arc()
                .startAngle(function(d) { return d.x; })
                .endAngle(function(d) { return d.x + d.dx; })
                .innerRadius(function(d) { return Math.sqrt(d.y); })
                .outerRadius(function(d) { return Math.sqrt(d.y + d.dy); });



            // Load data
            // ------------------------------

            d3.json("../../../../demo_data/d3/sunburst/sunburst_basic.json", function(error, root) {

                // Add sunbirst
                var path = svg.datum(root).selectAll("path")
                    .data(partition.nodes)
                    .enter()
                    .append("path")
                        .attr("class", "d3-slice-border")
                        .attr("display", function(d) { return d.depth ? null : "none"; }) // hide inner ring
                        .attr("d", arc)
                        .style("stroke-width", 1)
                        .style("fill", function(d) { return color((d.children ? d : d.parent).name); })
                        .style("fill-rule", "evenodd")
                        .each(stash);

                // Change data
                d3.selectAll(".basic-options input").on("change", function change() {
                    var value = this.value === "count"
                    ? function() { return 1; }
                    : function(d) { return d.size; };

                    // Transition
                    path.data(partition.value(value).nodes)
                        .transition()
                            .duration(750)
                            .attrTween("d", arcTween);
                });
            });


            // Stash the old values for transition.
            function stash(d) {
                d.x0 = d.x;
                d.dx0 = d.dx;
            }

            // Interpolate the arcs in data space.
            function arcTween(a) {
                var i = d3.interpolate({x: a.x0, dx: a.dx0}, a);
                return function(t) {
                    var b = i(t);
                    a.x0 = b.x;
                    a.dx0 = b.dx;
                    return arc(b);
                };
            }
        }
    };


    //
    // Return objects assigned to module
    //

    return {
        init: function() {
            _sunburstBasic();
        }
    }
}();


// Initialize module
// ------------------------------

document.addEventListener('DOMContentLoaded', function() {
    D3SunburstBasic.init();
});
