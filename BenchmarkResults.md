# Results of the TTC 2015 Train Benchmark

Last Run: 22.09.2016 15:03:44

Test Case|Master|Immediate|Transaction|Parallel|M→I|M→T|M→P|I→T|I→P|T→P|
---------|------|---------|-----------|--------|---|---|---|---|---|---|
PosLength|741.051 ns|120.669 ns|175.978 ns|288.168 ns|6,14x|4,21x|2,57x|0,69x|0,42x|0,61x|
RouteSensor|212 ns|204 ns|143 ns|148 ns|1,04x|1,49x|1,43x|1,43x|1,38x|0,96x|
SemaphoreNeighbor|15.782.400 ns|15.579.622 ns|14.638.492 ns|16.934.091 ns|1,01x|1,08x|0,93x|1,06x|0,92x|0,86x|
SwitchSensor|224 ns|208 ns|104 ns|107 ns|1,08x|2,15x|2,09x|1,99x|1,94x|0,97x|
SwitchSet|29.579 ns|3.753 ns|3.425 ns|8.609 ns|7,88x|8,64x|3,44x|1,10x|0,44x|0,40x|
