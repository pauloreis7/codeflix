package com.pauloreis.catalog.admin.infrastructure.api;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.mock.mockito.MockBean;
import org.springframework.test.web.servlet.MockMvc;

import com.pauloreis.catalog.admin.ControllerTest;
import com.pauloreis.catalog.admin.application.category.create.CreateCategoryUseCase;

@ControllerTest(controllers = CategoryAPI.class)
public class CategoryAPITest {

  @Autowired
  private MockMvc mvc;

  @MockBean
  private CreateCategoryUseCase createCategoryUseCase;

  @Test
  public void test() {

  }
}