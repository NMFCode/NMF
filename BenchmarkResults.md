# Results of the TTC 2015 Train Benchmark

Last Run: Sep 9, 2016 19:30

 Test Case         |  Master | Immediate | Transaction | M->I | M->T | I->T |
------------------ |-------- |---------- |------------ |----- |----- |----- |
 PosLength         | 6,44e-6 | 3,62e-6   | 3,27e-6     | 1,78 | 1,97 | 1,11 |
 RouteSensor       | 4,54e-7 | 4,54e-7   | 4,53e-7     | 1,00 | 1,00 | 1,00 |
 SemaphoreNeighbor | N/A*    | 1,57e-2   | 1,52e-2     | N/A* | N/A* | 1,04 |
 SwitchSensor      | 4,93e-7 | 4,89e-7   | 4,43e-7     | 1,01 | 1,11 | 1,10 |
 SwitchSet         | 4,02e-6 | 1,70e-6   | 1,84e-6     | 2,37 | 2,18 | 0,92 |

* Note: SemaphoreNeighbor does not currently run correctly on master.