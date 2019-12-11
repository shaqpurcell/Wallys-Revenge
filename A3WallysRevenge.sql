/*
FILE:           A3WallysRevenge.sql
PROJECT:	    Assignment #3 - Wally's Revenge
PROGRAMMER:     Shaq Purcell
FIRST VERSION:  2019-12-06
*/

-- Create the database

DROP DATABASE IF EXISTS SPwally;
CREATE DATABASE IF NOT EXISTS SPwally;

USE SPwally;


-- Reload privilages
-- 	FLUSH PRIVILEGES;

-- Drop user
-- DROP USER IF EXISTS 'root'@'localhost';

-- CREATE USER IF NOT EXISTS 'root'@'localhost' IDENTIFIED BY 'Conestoga1';

-- GRANT ALL PRIVILEGES ON SPWally.* TO 'root'@'localhost';

-- -----------------------------------------------------
-- Table `branches`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `branches` (
  `branchID` INT NOT NULL,
  `location` MEDIUMTEXT NULL DEFAULT NULL,
  `name` MEDIUMTEXT NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `orders`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `orders` (
  `orderID` INT NOT NULL,
  `customerID` INT NOT NULL,
  `branchID` INT NOT NULL,
  `orderStatus` MEDIUMTEXT NULL DEFAULT NULL,
  `orderDate` VARCHAR(40) NULL DEFAULT NULL,
  `returnDate` MEDIUMTEXT NULL DEFAULT NULL,
  `sPrice` DOUBLE)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `customer`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `customers` (
  `customerID` INT NOT NULL,
  `firstName` MEDIUMTEXT NULL DEFAULT NULL,
  `lastName` MEDIUMTEXT NULL DEFAULT NULL,
  `phoneNumber` MEDIUMTEXT NULL DEFAULT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;

-- -----------------------------------------------------
-- Table `products`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `products` (
  `productID` INT NOT NULL,
  `name` MEDIUMTEXT NULL DEFAULT NULL,
  `wPrice` FLOAT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `branchproducts`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `branchproducts` (
  `branchID` INT NOT NULL,
  `productID` INT NOT NULL,
  `quantity` INT NOT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;


-- -----------------------------------------------------
-- Table `orderline`
-- -----------------------------------------------------
CREATE TABLE IF NOT EXISTS `orderline` (
  `orderLineID` INT NOT NULL,
  `orderID` INT NOT NULL,
  `productID` INT NOT NULL,
  `amount` INT NOT NULL)
ENGINE = InnoDB
DEFAULT CHARACTER SET = latin1;



-- Add branches to datbase
INSERT INTO branches (branchID,location,`name`) VALUES 
 (1,'Cambridge','Preston Wallys World'),
 (2,'Waterloo','Kitchener Wallys World'),
 (3,'Guelph','Wellington Wallys World');
 
 -- Add customers to database
INSERT INTO customers (customerID, firstName, lastName, phoneNumber) VALUES 
 (1, 'Jack', 'Ryan', '619-231-9583'),
 (2, 'James', 'Bond', '582-395-3961'),
 (3, 'Shaq', 'Purcell', '420-498-7832'),
 (4, 'Jane', 'Moe', '123-123-0987');
 
 -- Add Items to database
INSERT INTO products (productID, `name`, wPrice) VALUES 
	(1,'Disco Queen Wallpaper','12.94'),
	(2,'Princess Peach Wallpaper','7.23'),
	(3,'Merica Wallpaper','54.60'),
	(4,'Canada Wallpaper','100.00'),
	(5,'Duck Wallpaper','32.32'),
	(6,'Anchor Wallpaper','18.90');

-- Add Products to branches
INSERT INTO branchproducts (branchID, productID, quantity) VALUES
	(1, 1, 20),
    (1, 2, 30),
    (1, 5, 10),
    (1, 6, 7),
    (2, 2, 5),
    (2, 4, 11),
    (2, 6, 20),
    (3, 1, 9),
    (3, 2, 2),
    (3, 3, 5);
    
-- Add Orders
INSERT INTO orders (orderID, customerID, branchID, orderStatus, orderDate, returnDate, sPrice) VALUES
	(1, 1, 1, "PAID", "2012-12-30", "1-1-1", 0),
    (2, 1, 1, "PAID", "2012-12-30", "1-1-1", 0),
    (3, 2, 1, "PAID", "2012-12-30", "1-1-1", 0),
    (4, 1, 1, "RETURN", "2012-12-30", "2019-12-03", 0);


-- Add Orderlines
INSERT INTO orderline (orderLineID, orderID, productID, amount) VALUES
	(1, 1, 1, 2),
    (2, 1, 2, 6),
    (3, 1, 3, 4),
    (4, 2, 5, 5),
	(5, 3, 6, 2),
	(6, 3, 5, 1),
	(7, 4, 5, 0),
    (8, 4, 1, 0);
    
-- Calculate the sales price of all orders currently within the database
-- Should be a in a loop, had issues creating while loop

SELECT @total := AVG(wPrice * 1.4) FROM ((orderline
	INNER JOIN orders ON orderline.orderID = orders.orderID)
	INNER JOIN products ON orderline.productID = products.productID)
	WHERE orderline.orderID = 1
	GROUP BY orderline.orderID;
	UPDATE orders
	SET sPrice = @total
	WHERE orderID = 1;
    
    
SELECT @total := AVG(wPrice * 1.4) FROM ((orderline
	INNER JOIN orders ON orderline.orderID = orders.orderID)
	INNER JOIN products ON orderline.productID = products.productID)
	WHERE orderline.orderID = 2
	GROUP BY orderline.orderID;
	UPDATE orders
	SET sPrice = @total
	WHERE orderID = 2;

SELECT @total := AVG(wPrice * 1.4) FROM ((orderline
	INNER JOIN orders ON orderline.orderID = orders.orderID)
	INNER JOIN products ON orderline.productID = products.productID)
	WHERE orderline.orderID = 3
	GROUP BY orderline.orderID;
	UPDATE orders
	SET sPrice = @total
	WHERE orderID = 3;

SELECT @total := AVG(wPrice * 1.4) FROM ((orderline
	INNER JOIN orders ON orderline.orderID = orders.orderID)
	INNER JOIN products ON orderline.productID = products.productID)
	WHERE orderline.orderID = 4
	GROUP BY orderline.orderID;
	UPDATE orders
	SET sPrice = @total
	WHERE orderID = 4;

-- Set primary keys for each table
ALTER TABLE customers MODIFY customerID INT AUTO_INCREMENT PRIMARY KEY;
ALTER TABLE products MODIFY productID INT AUTO_INCREMENT PRIMARY KEY;
ALTER TABLE orders MODIFY orderID INT AUTO_INCREMENT PRIMARY KEY;
ALTER TABLE branches MODIFY branchID INT AUTO_INCREMENT PRIMARY KEY;
ALTER TABLE orderline MODIFY orderLineID INT AUTO_INCREMENT PRIMARY KEY;
ALTER TABLE branchproducts ADD PRIMARY KEY (branchID, productID);


-- Create foreign key relationships
ALTER TABLE branchproducts
    ADD CONSTRAINT FOREIGN KEY (branchID)
    REFERENCES branches (branchID);

ALTER TABLE branchproducts
    ADD  CONSTRAINT FOREIGN KEY (productID)
    REFERENCES products (productID);

ALTER TABLE orderline
	ADD CONSTRAINT FOREIGN KEY (orderID)
    REFERENCES orders (orderID);

ALTER TABLE orderline
    ADD  CONSTRAINT FOREIGN KEY (productID)
    REFERENCES products (productID);
    
ALTER TABLE orders
    ADD  CONSTRAINT FOREIGN KEY (customerID)
    REFERENCES customers (customerID);
    
ALTER TABLE orders
    ADD  CONSTRAINT FOREIGN KEY (branchID)
    REFERENCES branches (branchID);
    
    

    
