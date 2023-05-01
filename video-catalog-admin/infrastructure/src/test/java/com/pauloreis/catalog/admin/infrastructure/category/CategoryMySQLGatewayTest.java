package com.pauloreis.catalog.admin.infrastructure.category;

import org.junit.jupiter.api.Assertions;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;

import com.pauloreis.catalog.admin.infrastructure.MySQLGatewayTest;
import com.pauloreis.catalog.admin.infrastructure.category.persistence.CategoryRepository;

@MySQLGatewayTest
public class CategoryMySQLGatewayTest {

  @Autowired
  private CategoryMySQLGateway categoryGateway;

  @Autowired
  private CategoryRepository categoryRepository;

  @Test
  public void testInjectedDependencies() {
    Assertions.assertNotNull(categoryGateway);
    Assertions.assertNotNull(categoryRepository);
  }
}