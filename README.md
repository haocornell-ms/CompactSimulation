Simulate write and read multiplification of compact in levelDB style. Currently, it simulates a situation where records are kept inserted with key evenly distributed in the key space. As a file at level n will merge with 10 files at level n+1, with 10 levels, if all levels are filled. a record is re-read and re-wrote around 100x and this program verifies that.